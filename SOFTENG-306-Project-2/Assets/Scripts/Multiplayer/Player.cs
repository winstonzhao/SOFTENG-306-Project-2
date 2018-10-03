using System;
using UnityEngine;

namespace Multiplayer
{
    [Serializable]
    public class Player
    {
        [SerializeField]
        private string username;

        [SerializeField]
        private string scene;

        [SerializeField]
        private float x;

        [SerializeField]
        private float y;

        [SerializeField]
        private float z;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Scene
        {
            get { return scene; }
            set { scene = value; }
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        public override string ToString()
        {
            return "Player{Username=" + Username + ", X=" + X + ", Y=" + Y + ", Z=" + Z + "};";
        }
    }
}
