using System;
using System.Collections.Generic;

namespace Multiplayer
{
    [Serializable]
    public class GameSync
    {
        public int lastChatMessageId;

        public List<Player> players;
    }
}
