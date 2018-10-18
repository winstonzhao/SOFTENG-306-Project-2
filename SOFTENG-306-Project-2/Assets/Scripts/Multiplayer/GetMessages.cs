using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Request that is sent to the server through the web socket to ask for more messages.
    ///
    /// This also doubles as a response by the server.
    /// </summary>
    [Serializable]
    public class GetMessages
    {
        [SerializeField]
        private int sinceMessageId;

        [SerializeField]
        private int limit;

        [SerializeField]
        private List<ChatMessage> messages;

        /// <summary>
        /// Give me messages after this message id
        /// </summary>
        public int SinceMessageId
        {
            get { return sinceMessageId; }
            set { sinceMessageId = value; }
        }

        /// <summary>
        /// I only want this many messages
        /// </summary>
        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        /// <summary>
        /// Set by the server when it responds with the requested messages
        /// </summary>
        public List<ChatMessage> Messages
        {
            get { return messages; }
            set { messages = value; }
        }
    }
}
