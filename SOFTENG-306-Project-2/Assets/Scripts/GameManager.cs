using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private Player _player = new Player();

	public void ChangeScene(string levelName)
	{
        Initiate.Fade(levelName, new Color(0.968f, 0.835f, 0.403f), 1.0f);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public Player Player
	{
		get { return _player; }
		set { _player = value; }
	}
}
