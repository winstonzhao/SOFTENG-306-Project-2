using System;
using Game;
using Game.Hiscores;
using UnityEngine;
using UnityEngine.UI;

public class SoftwareEndScreen : MonoBehaviour
{
    public Canvas InstructionCanvas;
    public DraggableScrollList scrollList;
    public Text ExitText;
    private string defaultEndText = "Well Done!\n\nYou have now completed the Software Minigame!";

    private static int maxScore = 100;
    private int instructionCount;
    // Use this for initialization
    void Start()
    {
        Open();
    }

    public void Open()
    {
        instructionCount = scrollList.listItems.Count;
        gameObject.SetActive(true);

        // Hide instruction elements
        InstructionCanvas.gameObject.SetActive(false);

        // Personalise feedback
        ExitText.text = defaultEndText + "\n\nYou used " + instructionCount + " instructions " +
                        "and scored " + (maxScore - instructionCount) +
                        "!\n\n(Use less instructions to score more)";
    }

    public void Close()
    {
        InstructionCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
        ExitText.text = defaultEndText;
    }

    /// <summary>
    /// Links the minigame back to the lobby and saves the highscore
    /// </summary>
    public void EndMiniGame()
    {
       // Get the current level of the game
        int level = GameObject.Find("GameManager").GetComponent<SoftwareLevelGenerator>().currentLevel;
        int expected = 0;
        int s = maxScore;

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
                expected = 10;
                break;
        }
        
        // For extra instruction used, -10 for score
        for (int i = 0; i < instructionCount; i++)
        {
            if (i > expected)
            {
                s -= 10;
            }
        }

        // Set to 0 if score gets to negative
        if (s < 0)
        {
            s = 0;
        }

        Debug.Log("Score is: " + s);
        
        // Add score to the list of scores for software game
        var score = new Score()
        {
            CreatedAt = DateTime.Now,
            Minigame =  Minigames.Software,
            Value = s
        };

        Toolbox.Instance.Hiscores.Add(score);

        Toolbox.Instance.GameManager.ChangeScene("Engineering Leech");
    }
}