using System;
using Multiplayer;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GameSettings
    {
        [NonSerialized]
        public bool IsDirty;

        [SerializeField]
        private bool hasBeenGreeted;
        public bool HasBeenGreeted
        {
            get { return hasBeenGreeted; }
            set
            {
                hasBeenGreeted = value;
                IsDirty = true;
            }
        }

        [SerializeField]
        private Player player;
        public Player Player
        {
            get { return player; }
            set
            {
                player = value;
                IsDirty = true;
            }
        }
    }
}
