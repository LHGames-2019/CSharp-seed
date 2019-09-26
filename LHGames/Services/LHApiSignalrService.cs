using LHGames.Helper;
using LHGames.Helpers;
using LHGames.Services.Interfaces;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using PolyHx.ApiClients.Sts.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Services
{
    public class LHApiSignalrService : ISignalrService
    {
        private readonly GameServerSignalrService _gameserverSignalrService;
        private readonly IOptions<AppSettings> _appSettings;

        public HubConnection Connection { get; set; }

        public LHApiSignalrService(GameServerSignalrService gameserverSignalrService,
                                   IOptions<AppSettings> appSettings)
        {
            _gameserverSignalrService = gameserverSignalrService;
            _appSettings = appSettings;
        }

        public async Task ConnectAsync()
        {
            Connection = new HubConnectionBuilder()
            .WithUrl($"{Environment.GetEnvironmentVariable("LHAPI_URL")}/teamshub", options =>
            {
                options.Transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents;
            })
            .Build();
            try
            {
                await Connection.StartAsync();

            } catch (System.Net.Http.HttpRequestException)
            {
                await _gameserverSignalrService.ConnectAsync(_appSettings.Value.GS_URL);
            }

            InitiateCallbacks();
        }

        public void InitiateCallbacks()
        {
            Connection.On(Constants.SignalRFunctionNames.AssignGameServerUriToGameId, 
                        async (string gameserverUri) => await _gameserverSignalrService.ConnectAsync(gameserverUri));

            Connection.On(Constants.SignalRFunctionNames.AssignTeamId, (string teamId) => _gameserverSignalrService.TeamId = teamId);
        }
    }
}
