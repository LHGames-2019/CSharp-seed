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
        public Direction ExecuteTurn(GameInfo gameInfo)
        {

            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine("Sleep for 1 second!");
            //    Thread.Sleep(1000);
            //}

            while (true)
            {
                Console.WriteLine("Sleeping");
            }

            return Direction.Up;
        }
    }
}
