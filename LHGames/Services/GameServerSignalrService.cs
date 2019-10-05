using LHGames.Helpers;
using LHGames.Services.Interfaces;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using PolyHx.ApiClients.Sts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using LHGames.Helper;
using LHGames.Bot;
using Microsoft.Extensions.Options;
using System.Threading;

namespace LHGames.Services
{
    /// <summary>
    /// !!! DO NOT EDIT !!!
    /// </summary>
    public class GameServerSignalrService : ISignalrService
    {
        private readonly IOptions<AppSettings> _appSettings;

        public HubConnection Connection { get; set; }

        static readonly PlayerBot PlayerBot = new PlayerBot();

        public GameServerSignalrService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task ConnectAsync()
        {
            string s = $"{_appSettings.Value.GS_URL}/teamshub";
            Connection = new HubConnectionBuilder()
            .WithUrl($"{_appSettings.Value.GS_URL}/teamshub", options =>
            {
                options.Transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents;
            })
            .Build();

            await Connection.StartAsync().ContinueWith(res =>
            {
                Connection.InvokeAsync(Constants.SignalRFunctionNames.Register, Environment.GetEnvironmentVariable("TEAM_ID") ?? "");
            });

            InitiateCallbacks();
        }

        public async Task ConnectAsync(string uri)
        {
            Connection = new HubConnectionBuilder()
            .WithUrl($"{uri}/teamshub", options =>
            {
                options.Transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents;
            })
            .Build();

            await Connection.StartAsync().ContinueWith(res =>
            {
                Connection.InvokeAsync(Constants.SignalRFunctionNames.Register, Environment.GetEnvironmentVariable("TEAM_ID") ?? "");
            });

            InitiateCallbacks();
        }

        public void InitiateCallbacks()
        {
            Connection.On(Constants.SignalRFunctionNames.RequestExecuteTurn,
                        (string[] currentMap, int dimension, int maxMovement, int movementLeft, Direction lastMove, int teamNumberi) 
                        => RequestExecuteTurn(currentMap, dimension, maxMovement, movementLeft, lastMove, teamNumberi));
            Connection.On(Constants.SignalRFunctionNames.ReceiveFinalMap, (string[] currentMap) => { ReceiveFinalMap(currentMap); });
        }

        public async void ReceiveFinalMap(string[] currentMap)
        {
            await Connection.StopAsync();
        }

        public async void RequestExecuteTurn( string[] currentMap, int dimension, int maxMovement, int movementLeft,  Direction lastMove, int teamNumber)
        {

            GameInfo gameInfo = new GameInfo() {
                Map = currentMap,
                Self = new HostPlayer()
                {
                    TeamNumber = teamNumber,
                    MaxMovement = maxMovement,
                    MovementLeft = movementLeft,
                    LastMove = lastMove,
                    Position = HelperFunctions.GetPositionByTeamNumber(currentMap, dimension, teamNumber),
                    SizeOfBody = HelperFunctions.GetSizeOfBodyByTeamNumber(currentMap, teamNumber),
                    SizeOfTail = HelperFunctions.GetSizeOfTailByTeamNumber(currentMap, teamNumber)
                },
                Others = new List<OtherPlayer>()
            };

            int[] possibleId = { 1, 2, 3, 4 };

            foreach (int id in possibleId.Where(i => i != teamNumber))
            {
                gameInfo.Others.Add(new OtherPlayer()
                {
                    TeamNumber = id,
                    Position = HelperFunctions.GetPositionByTeamNumber(currentMap, dimension, id),
                    SizeOfBody = HelperFunctions.GetSizeOfBodyByTeamNumber(currentMap, id),
                    SizeOfTail = HelperFunctions.GetSizeOfTailByTeamNumber(currentMap, id)
                });
            }

            try
            {
                Direction nextMove = Task<Direction>.Run(() => { return PlayerBot.ExecuteTurn(gameInfo); }).Result; 
                HelperFunctions.Print2DMap(HelperFunctions.Get2DMap(currentMap, dimension));
                Console.WriteLine($"___________ Turn Executed with move: {nextMove} ___________");
                await Connection.InvokeAsync(Constants.SignalRFunctionNames.ReturnExecuteTurn, nextMove);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

    }
}
