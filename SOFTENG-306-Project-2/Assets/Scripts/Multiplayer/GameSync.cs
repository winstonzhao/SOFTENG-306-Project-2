using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Sent by the server on every game tick to tell us about the current state of all the other players.
    /// </summary>
    [Serializable]
    public class GameSync
    {
        [SerializeField]
        private string currentTime;

        [SerializeField]
        private int lastChatMessageId;

        [SerializeField]
        private List<Player> players;

        /// <summary>
        /// The current game time - used to calculate time drift between the client and server
        /// </summary>
        public string CurrentTime
        {
            get { return currentTime; }
            set { currentTime = value; }
        }

        /// <summary>
        /// The newest chat id - used to determine whether we need to fetch new messages from the server
        /// </summary>
        public int LastChatMessageId
        {
            get { return lastChatMessageId; }
            set { lastChatMessageId = value; }
        }

        /// <summary>
        /// All of the players connected to the server - possibly filtered by scene; no guarantees
        /// </summary>
        public List<Player> Players
        {
            get { return players; }
            set { players = value; }
        }
    }
}
