using System;
using System.Collections.Generic;

namespace Multiplayer
{
    [Serializable]
    public class GetMessages
    {
        public int sinceMessageId;

        public int limit;

        public List<ChatMessage> messages;
    }
}
