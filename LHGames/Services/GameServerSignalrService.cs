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
    public class GameServerSignalrService : ISignalrService
    {
        private readonly IOptions<AppSettings> _appSettings;

        public HubConnection Connection { get; set; }

        static readonly PlayerBot PlayerBot = new PlayerBot();

        public string TeamId { get; set; } = "";

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
                Connection.InvokeAsync(Constants.SignalRFunctionNames.Register, TeamId);
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
                Connection.InvokeAsync(Constants.SignalRFunctionNames.Register, TeamId);
            });

            InitiateCallbacks();
        }

        public void InitiateCallbacks()
        {
            Connection.On(Constants.SignalRFunctionNames.RequestExecuteTurn,
                        (string[] currentMap, int dimension, int maxMovement, int movementLeft, Direction lastMove, int teamNumberi) 
                        => RequestExecuteTurn(currentMap, dimension, maxMovement, movementLeft, lastMove, teamNumberi));
        }

        public void RequestExecuteTurn( string[] currentMap, int dimension, int maxMovement, int movementLeft,  Direction lastMove, int teamNumber)
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
                    TeamNumber = teamNumber,
                    Position = HelperFunctions.GetPositionByTeamNumber(currentMap, dimension, id),
                    SizeOfBody = HelperFunctions.GetSizeOfBodyByTeamNumber(currentMap, id),
                    SizeOfTail = HelperFunctions.GetSizeOfTailByTeamNumber(currentMap, id)
                });
            }

            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            Task<Direction> task = Task.Run(() => { return PlayerBot.ExecuteTurn(gameInfo); }, cancellationToken.Token);

            try
            {
                Direction nextMove = TimeoutAfter(task, 1000, cancellationToken);
                HelperFunctions.Print2DMap(HelperFunctions.Get2DMap(currentMap, dimension));
                Console.WriteLine($"{HelperFunctions.GetPositionByTeamNumber(currentMap, dimension, teamNumber).X} : {HelperFunctions.GetPositionByTeamNumber(currentMap, dimension, teamNumber).Y}");
                Console.WriteLine(HelperFunctions.GetSizeOfBodyByTeamNumber(currentMap, teamNumber));
                Console.WriteLine(HelperFunctions.GetSizeOfTailByTeamNumber(currentMap, teamNumber));
                Console.WriteLine(lastMove);
                Console.WriteLine(nextMove);
                Connection.InvokeAsync(Constants.SignalRFunctionNames.ReturnExecuteTurn, nextMove);
            } catch(Exception e)
            {
                //cancellationToken.Cancel();    
                var b = task.IsCanceled;
                Console.WriteLine(e.Message);
            }
            finally
            {
                cancellationToken.Dispose();
            }

        }

        private static T TimeoutAfter<T>(Task<T> task, int millisecondsTimeout, CancellationTokenSource cancellationToken)
        {
            bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(millisecondsTimeout));
            if (isCompletedSuccessfully)
                return task.Result;
            else
                cancellationToken.Cancel();
                throw new TimeoutException();
        }
    }
}
