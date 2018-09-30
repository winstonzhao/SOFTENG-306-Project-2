using System;

namespace Multiplayer
{
    [Serializable]
    public class Player
    {
        public string username;

        public string scene;

        public float x;

        public float y;

        public float z;

        public override string ToString()
        {
            return "Player{username=" + username + ", x=" + x + ", y=" + y + ", z=" + z + "};";
        }
    }
}
