using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    [Serializable]
    public class GameSync
    {
        [SerializeField]
        private string currentTime;

        [SerializeField]
        private int lastChatMessageId;

        [SerializeField]
        private List<Player> players;

        public string CurrentTime
        {
            get { return currentTime; }
            set { currentTime = value; }
        }

        public int LastChatMessageId
        {
            get { return lastChatMessageId; }
            set { lastChatMessageId = value; }
        }

        public List<Player> Players
        {
            get { return players; }
            set { players = value; }
        }
    }
}
