using System;
using Game;
using Game.Hiscores;

namespace SoftwareMinigame
{
    public class SoftwareSingleton : Singleton<SoftwareSingleton>
    {
        public int TotalScore { get; set; }

        public int LevelsPassed { get; set; }

        /// <summary>
        /// To be called when exiting the game
        /// Handles saving of scores
        /// </summary>
        public void FinishGame()
        {
            if (LevelsPassed > 0)
            {
                // Add score to the list of scores for software game
                var newScore = new Score()
                {
                    CreatedAt = DateTime.Now,
                    Minigame = Minigames.Software,
                    Value = (int) (TotalScore / LevelsPassed)
                };

                Toolbox.Instance.Hiscores.Add(newScore);
            }

            Toolbox.Instance.GameManager.ChangeScene("Engineering Leech");
        }
    }
}