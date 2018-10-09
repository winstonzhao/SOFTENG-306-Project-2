using System;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Sent once by the server during the connection handshake to establish information
    /// </summary>
    [Serializable]
    public class GameInitialization
    {
        [SerializeField]
        private string currentDateTime;

        [SerializeField]
        private int tickPeriod;

        /// <summary>
        /// The server's current time given in ISO 8601. 
        /// Used to calculate time drift between the client and server
        /// </summary>
        public string CurrentDateTime
        {
            get { return currentDateTime; }
            set { currentDateTime = value; }
        }

        /// <summary>
        /// The time in milliseconds between each game tick 
        /// </summary>
        public int TickPeriod
        {
            get { return tickPeriod; }
            set { tickPeriod = value; }
        }
    }
}
