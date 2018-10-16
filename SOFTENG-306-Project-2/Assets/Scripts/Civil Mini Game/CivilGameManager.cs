using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Hiscores;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is a singleton class for managing the civil mini game across multiple levels.
 * This class also includes a few util methods that can be used independently by any level.
 */
public class CivilGameManager : MonoBehaviour {

    // singleton CivilGameManager instance
    public static CivilGameManager instance;
    
    // player name, default is "Anonymous"
    public string playerName  = "Anonymous";

    private Dictionary<int, int> scores = new Dictionary<int, int>();
    
    private void Awake()
    {
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

        // set the player name
        SetPlayerName(Toolbox.Instance.GameManager.Player.Username);
    }

    public void SetPlayerName(string playerName)
    {
        instance.playerName = playerName;
    }

    public void AddScore(int score, int level)
    {
        if (!scores.ContainsKey(level) || score > scores[level])
        {
            scores[level] = score;
        }
    }

    public void AddHighScore()
    {
        if (scores.Count > 0)
        {
            Score score = new Score();
            score.Minigame = Minigames.Civil;
            int highScore = (int) scores.Values.Average();
            score.Value = highScore;
            score.CreatedAt = DateTime.Now;

            Toolbox.Instance.Hiscores.Add(score);
            scores.Clear();
        }
    }

    public static void ToggleDialogDisplay(Canvas canvas, string groupName, bool show)    // super
    {
        GameObject group = FindObject(canvas.gameObject, groupName).gameObject;
        if (group != null)
        {
            group.SetActive(show);
        }
    }

    /**
 * Find an object in a parent object including parent objects
 */
    public static GameObject FindObject(GameObject parent, string name)    // super
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
