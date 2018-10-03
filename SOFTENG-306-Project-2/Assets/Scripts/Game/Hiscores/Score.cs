using System;

namespace Game.Hiscores
{
    [Serializable]
    public class Score
    {
        public Minigames Minigame;

        public float Value;

        public DateTime CreatedAt;

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
