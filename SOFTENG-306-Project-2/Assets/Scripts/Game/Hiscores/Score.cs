using System;
using UnityEngine;

namespace Game.Hiscores
{
    [Serializable]
    public class Score
    {
        public Minigames Minigame;

        public float Value;

        [SerializeField]
        private string createdAt;
        public DateTime CreatedAt
        {
            get
            {
                return DateTime.Parse(createdAt);
            }
            set
            {
                createdAt = value.ToLongDateString();

            }
        }

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
