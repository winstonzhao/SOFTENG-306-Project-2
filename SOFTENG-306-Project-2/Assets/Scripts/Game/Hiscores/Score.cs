using System;
using UnityEngine;

namespace Game.Hiscores
{
    [Serializable]
    public class Score
    {
        /// <summary>
        /// Which minigame the score belongs to
        /// </summary>
        public Minigames Minigame;

        /// <summary>
        /// The score the user got
        /// </summary>
        public float Value;

        [SerializeField]
        private string createdAt;
        /// <summary>
        /// The time the score was created at - when the user achieved the score
        /// </summary>
        public DateTime CreatedAt
        {
            get { return DateTime.Parse(createdAt); }
            set { createdAt = value.ToLongDateString(); }
        }

        /// <summary>
        /// Any other features you wish to display in the score view.
        ///
        /// Or any more information you wish to save.
        /// </summary>
        public object Extras;

        public override string ToString()
        {
            return "Score{Minigame=" + Minigame
                                     + ", Value=" + Value
                                     + ", CreatedAt=" + CreatedAt
                                     + ", Extras=" + Extras
                                     + "}";
        }
    }
}
