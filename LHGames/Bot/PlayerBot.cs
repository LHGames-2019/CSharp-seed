using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LHGames.Helper;

namespace LHGames.Bot
{
    public class PlayerBot
    {
        public PlayerBot() { }

        /// <summary>
        /// Implement your bot here.
        /// </summary>
        public async Task<Direction> ExecuteTurn(GameInfo gameInfo)
        {
            return Direction.Up;
        }
    }
}
