using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
    private Player _player = new Player();

	
	private void Awake()
	{
		Debug.Log("gameManager Alive");
		//Check if instance already exists
		if (instance == null)
                
			//if not, set instance to this
			instance = this;
            
		//If instance already exists and it's not this:
		else if (instance != this)
                
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    
            
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}

	public void ChangeScene(string levelName)
	{
		SceneManager.LoadScene(levelName);
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
