using System;

namespace Multiplayer
{
    [Serializable]
    public class ChatMessage
    {
        public int id;

        public string owner;

        public string message;

        public string sentAt;

        [NonSerialized]
        public DateTime sentAtDateTime;
    }
}
