using System;
using Multiplayer;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GameSettings
    {
        /// <summary>
        /// Whether there have been changes to this object that require it to be saved to disk
        /// </summary>
        [NonSerialized]
        public bool IsDirty;

        [SerializeField]
        private bool hasBeenGreeted;
        /// <summary>
        /// Determines whether the user has been greeted by the greeter NPC
        /// </summary>
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
        /// <summary>
        /// Our current player object, note that this object is mutable; any changes to the player object directly
        /// may not be automatically saved. The manager for game settings saves periodically but won't know if you
        /// modify the player object directly.
        ///
        /// If you need to force a save then set the field to mark the game settings as dirty 
        /// </summary>
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
