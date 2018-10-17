using System;
using Game;
using Game.Hiscores;
using UnityEngine;

public class ElectricalEnd : MonoBehaviour
{
    private int gameScore;

	public void Finish()
	{
        // get the score for the plaer
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
