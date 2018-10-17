using System;
using Game;
using Game.Hiscores;
using Instructions;
using SoftwareMinigame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoftwareEndScreen : MonoBehaviour
{
    public Canvas InstructionCanvas;
    public DraggableScrollList scrollList;
    private static string LEVEL_PREFIX = "scenes/Software Level ";
    public Text ScoreText;

    private static int maxScore = 100;
    private int instructionCount;
    private int score;

    // Use this for initialization
    void Start()
    {
        Open();
    }

    /// <summary>
    /// Opens the end screen, hiding the instruction canvas
    /// </summary>
    public void Open()
    {
        if (ScoreText == null) ScoreText = transform.GetChild(1).GetComponent<Text>();

        if (scrollList == null)
        {
            scrollList = FindObjectOfType<InstructionExecutor>().GetComponent<DraggableScrollList>();
        }

        instructionCount = scrollList.listItems.Count;
        CalculateScore();

        gameObject.SetActive(true);

        if (InstructionCanvas == null)
        {
            InstructionCanvas = GameObject.Find("InstructionCanvas").GetComponent<Canvas>();
        }

        // Hide instruction elements
        InstructionCanvas.gameObject.SetActive(false);

        int level = GameObject.Find("GameManager").GetComponent<SoftwareLevelGenerator>().currentLevel;
        if (level == 1)
        {
            SoftwareSingleton.Instance.TotalScore = 0;
            SoftwareSingleton.Instance.LevelsPassed = 0;
        }

        //Personalise feedback
        ScoreText.text = "Score:" + score + " Average:" +
                         (int) ((score + SoftwareSingleton.Instance.TotalScore) /
                                (SoftwareSingleton.Instance.LevelsPassed + 1));
    }

    private void CalculateScore()
    {
        int level = GameObject.Find("GameManager").GetComponent<SoftwareLevelGenerator>().currentLevel;
        int expected = 0;
        score = maxScore;

        // Set expected number of instruction used
        switch (level)
        {
            case 1:
                expected = 2;
                break;
            case 2:
                expected = 4;
                break;
            case 3:
                expected = 6;
                break;
            case 4:
                expected = 5;
                break;
            case 5:
                expected = 6;
                break;
            case 6:
                expected = 12;
                break;
        }

        // For extra instruction used, -10 for score
        for (int i = 0; i < instructionCount; i++)
        {
            if (i >= expected)
            {
                score -= 10;
            }
        }

        // Set to 0 if score gets to negative
        if (score < 0)
        {
            score = 0;
        }
    }

    public void Close()
    {
        InstructionCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
//        ExitText.text = defaultEndText;
    }

    public void QuitGame()
    {
        SoftwareSingleton.Instance.FinishGame();
    }

    // Reload the current level
    public void Restart()
    {
        int level = GameObject.Find("GameManager").GetComponent<SoftwareLevelGenerator>().currentLevel;

        SceneManager.LoadScene(LEVEL_PREFIX + level);
    }

    /// <summary>
    /// Skips to the next level
    /// </summary>
    public void SkipLevel()
    {
        int level = GameObject.Find("GameManager").GetComponent<SoftwareLevelGenerator>().currentLevel;
        SceneManager.LoadScene(LEVEL_PREFIX + (level + 1));
    }

    /// <summary>
    /// Links the minigame back to the lobby and saves the highscore
    /// </summary>
    public void EndLevel()
    {
       // Get the current level of the game
        int level = GameObject.Find("GameManager").GetComponent<SoftwareLevelGenerator>().currentLevel;

        SoftwareSingleton.Instance.TotalScore += score;
        SoftwareSingleton.Instance.LevelsPassed += 1;

        if (level < 6)
        {
            SceneManager.LoadScene(LEVEL_PREFIX + (level + 1));
        }
        else
        {
            SoftwareSingleton.Instance.FinishGame();
        }

    }
}