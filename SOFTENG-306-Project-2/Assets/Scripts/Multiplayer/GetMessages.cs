using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    [Serializable]
    public class GetMessages
    {
        [SerializeField]
        private int sinceMessageId;

        [SerializeField]
        private int limit;

        [SerializeField]
        private List<ChatMessage> messages;

        public int SinceMessageId
        {
            get { return sinceMessageId; }
            set { sinceMessageId = value; }
        }

        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        public List<ChatMessage> Messages
        {
            get { return messages; }
            set { messages = value; }
        }
    }
}
