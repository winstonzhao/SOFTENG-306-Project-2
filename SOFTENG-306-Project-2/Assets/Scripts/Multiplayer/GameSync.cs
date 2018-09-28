using System;
using System.Collections.Generic;

namespace Multiplayer
{
    [Serializable]
    public class GameSync
    {
        public int iteration;

        public List<Player> players;
    }
}
