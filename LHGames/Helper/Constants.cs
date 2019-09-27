using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Helpers
{
    abstract public class Constants
    {
        /// <summary>
        /// !!! DO NOT EDIT !!!
        /// </summary>
        public class SignalRFunctionNames
        {
            public static string AssignGameServerUriToGameId = "AssignGameServerUriToGameId";

            public static string AssignTeamId = "AssignTeamId";

            public static string RequestExecuteTurn = "RequestExecuteTurn";

            public static string ReturnExecuteTurn = "ReturnExecuteTurn";

            public static string Register = "Register";
        }
    }
}
