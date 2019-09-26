using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Services.Interfaces
{
    public interface ISignalrService
    {
        HubConnection Connection { get; set; }

        /// <summary>
        /// Initiate the connection with the signalr function
        /// </summary>
        /// <returns></returns>
        Task ConnectAsync();

        /// <summary>
        /// Initiate the callbacks functions
        /// </summary>
        void InitiateCallbacks();
    }
}
