using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Singleton class that holds the state of all scores for the user on the Electrical Game.
 */ 
namespace ElectricalScripts.Electrical_Scores
{
    public class Electrical_Scores : Singleton<Electrical_Scores>
    {
        public int levelsCompleted { get; private set; }
        public int totalScore { get; private set; }
        private int[,] levelScores = new int[,] { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 }, { 8, 0 }, { 9, 0 }, { 10, 0 }, { 11, 0 }, { 12, 0 } };
        private int totalLevels = 12;

        /*
         * Method called by the Electrical_Level Script to add to the players high score
         */ 
        public void addScore(int level, int score)
        {
            // if the current score for this level is better for a previous level
            if (levelScores[(level-1), 1] < score)
            {
                levelScores[(level - 1), 1] = score;
            }

            // if this is a new highest level achieve set the levels completed to this level
            if (levelsCompleted < level)
            {
                levelsCompleted = level;
            }

            // Debug to check the score is now added
            Debug.Log("Score is now:" + levelScores[(level - 1), 1]);
        }

        /*
         * Method called that returns the players average score over the levels.
         */ 
        public int getScore()
        {
            // iterate through the array to get the total score
            for (int i=0; i<totalLevels;i++)
            {
                totalScore += levelScores[i, 1];
            }

            if (totalScore == 0)
            {
                return totalScore;
            } else
            {
                // return the score divided by the number of levels completed
                return (totalScore / levelsCompleted);
            }
            
        }
        
        /*
         * Method that returns the number of levels the user has completed
         */ 
        public int getLevels()
        {
            return levelsCompleted;
        }
    }
}
