using System;

namespace Multiplayer
{
    /// <summary>
    /// Sent once by the server during the connection handshake to establish information
    /// </summary>
    [Serializable]
    public class GameInitialization
    {
        /// <summary>
        /// The server's current time given in ISO 8601. 
        /// Used to calculate time drift between the client and server
        /// </summary>
        public string currentDateTime;

        /// <summary>
        /// The time in milliseconds between each game tick 
        /// </summary>
        public int tickPeriod;
    }
}
