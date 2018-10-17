using System;
using Game;
using Game.Hiscores;
using UnityEngine;

/*
 * A script to send the player's highscore to the highscore board in the Menu
 * of the game after the player exits the minigame
 */
public class ElectricalEnd : MonoBehaviour
{
    private int gameScore;

	public void Finish()
	{
        // get the score for the player
        gameScore = Toolbox.Instance.Electrical_Scores.getScore();

        // create the score object
        var score = new Score()
		{
			Minigame = Minigames.Electrical,
			Value = gameScore,
			CreatedAt = DateTime.Now
		};

        // Add the highscore to the highscore board
		Toolbox.Instance.Hiscores.Add(score);

        // Change the scene to the Engineering Leech
		Toolbox.Instance.GameManager.ChangeScene("Engineering Leech");
	}
}
