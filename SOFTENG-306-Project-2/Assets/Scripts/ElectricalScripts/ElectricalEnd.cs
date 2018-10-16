using System;
using Game;
using Game.Hiscores;
using UnityEngine;

public class ElectricalEnd : MonoBehaviour
{
    private int gameScore;

	public void Finish()
	{
        gameScore = Toolbox.Instance.Electrical_Scores.getScore();


        var score = new Score()
		{
			Minigame = Minigames.Electrical,
			Value = gameScore,
			CreatedAt = DateTime.Now
		};
		Toolbox.Instance.Hiscores.Add(score);
		Toolbox.Instance.GameManager.ChangeScene("Engineering Leech");
	}
}
