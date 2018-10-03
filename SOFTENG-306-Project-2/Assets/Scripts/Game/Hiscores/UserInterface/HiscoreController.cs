using System;
using UnityEngine;
using WebSocketSharp;

namespace Game.Hiscores.UserInterface
{
    public class HiscoreController : MonoBehaviour
    {
        private Minigames minigame;

        public Minigames Minigame
        {
            get { return minigame; }
            set
            {
                minigame = value;

                OnMinigameChange.Emit(this, EventArgs.Empty);
            }
        }

        public event EventHandler OnMinigameChange;
    }
}
