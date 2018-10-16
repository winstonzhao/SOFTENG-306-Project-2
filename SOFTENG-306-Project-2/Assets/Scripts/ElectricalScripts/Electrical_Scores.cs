using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ElectricalScripts.Electrical_Scores
{
    public class Electrical_Scores : Singleton<Electrical_Scores>
    {
        public int levelsCompleted { get; private set; }
        public int totalScore { get; private set; }
        private int[,] levelScores = new int[,] { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 } };
        private int totalLevels = 6;

        public void addScore(int level, int score)
        {
            if (levelScores[(level-1), 1] < score)
            {
                levelScores[(level - 1), 1] = score;
            }

            if (levelsCompleted < level)
            {
                levelsCompleted = level;
            }

            Debug.LogWarning("Score is now:" + levelScores[(level - 1), 1]);
        }

        public int getScore()
        {
            for (int i=0; i<totalLevels;i++)
            {
                totalScore += levelScores[i, 1];
            }
           // foreach (int score in levelScores)
            //{
              //  totalScore += score;
            //}
            return (totalScore / totalLevels);
        }

        public int getLevel()
        {
            return levelsCompleted;
        }
    }
}
