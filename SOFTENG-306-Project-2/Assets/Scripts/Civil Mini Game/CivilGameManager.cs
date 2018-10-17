using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Hiscores;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * This class is a singleton class for managing the civil mini game across multiple levels.
 * This class also includes a few util methods that can be used independently by any level.
 */
public class CivilGameManager : MonoBehaviour
{
    // Singleton CivilGameManager instance
    public static CivilGameManager Instance;

    // Player name, default is "Anonymous"
    [FormerlySerializedAs("playerName")] public string PlayerName = "Anonymous";

    // Managing scores, key is Level Number and Value is Score
    private readonly Dictionary<int, int> _scores = new Dictionary<int, int>();

    private void Awake()
    {
        // Check if instance already exists
        if (Instance == null)

            // If not, set instance to this
            Instance = this;

        // If instance already exists and it's not this:
        else if (Instance != this)

            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        // Set the player name
        SetPlayerName(Toolbox.Instance.GameManager.Player.Username);
    }

    /**
     * Set the player name for the civil mini-game
     */
    public void SetPlayerName(string playerName)
    {
        Instance.PlayerName = playerName;
    }

    /**
     * Add a score to the scores for other levels
     */
    public void AddScore(int score, int level)
    {
        // If this is a new score for the level, or its greater than their previous attempt
        if (!_scores.ContainsKey(level) || score > _scores[level])
        {
            _scores[level] = score; // Add the new score
        }
    }

    /**
     * Add a new civil mini-game high score to the overall game high scores
     */
    public void AddHighScore()
    {
        if (_scores.Count > 0) // If they have achieved atleast 1 score
        {
            // Add a new high score for the mini-game as an average of their level scores.
            Score score = new Score();
            score.Minigame = Minigames.Civil;
            int highScore = (int) _scores.Values.Average();
            score.Value = highScore;
            score.CreatedAt = DateTime.Now; // Current Time

            Toolbox.Instance.Hiscores.Add(score); // Add the score
            _scores.Clear(); // Reset the scores for the mini-game
        }
    }
    
    /**
     * Calculate the high score for the current civil mini game
     */
    public int CalculateHighScore()
    {
        return _scores.Count > 0 ? (int) _scores.Values.Average() : 0;
    }

    /**
     * Toggle display based off a canvas that a panel is on, the name of the panel group and whether display is being
     * shown
     */
    public static void ToggleDialogDisplay(Canvas canvas, string groupName, bool show)
    {
        GameObject group = FindObject(canvas.gameObject, groupName).gameObject; // Find the panel in the canvas
        if (group != null)
        {
            group.SetActive(show); // Show the panel
        }
    }

    /**
     * Find an object in a parent object including parent and inactive objects
     */
    private static GameObject FindObject(GameObject parent, string name)
    {
        var trs = parent.GetComponentsInChildren<Transform>(true); // All objects
        foreach (Transform t in trs)
        {
            if (t.name == name) // The object we are looking for
            {
                return t.gameObject;
            }
        }

        return null;
    }
}