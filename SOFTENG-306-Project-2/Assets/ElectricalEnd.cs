using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Hiscores;
using UnityEngine;

public class ElectricalEnd : MonoBehaviour
{

	public void Finish()
	{
		var score = new Score()
		{
			Minigame = Minigames.Electrical,
			Value = 0,
			CreatedAt = DateTime.Now
		};
		Toolbox.Instance.Hiscores.Add(score);
		Toolbox.Instance.GameManager.ChangeScene("Engineering Leech");
	}
}
