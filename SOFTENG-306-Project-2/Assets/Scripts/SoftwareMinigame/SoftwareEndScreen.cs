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

        Toolbox.Instance.QuestManager.MarkFinished("software-workshop");
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
        var score = new Score()
        {
            CreatedAt = DateTime.Now,
            Minigame = Minigames.Software,
            Value = maxScore - instructionCount,
        };

        Toolbox.Instance.Hiscores.Add(score);

        Toolbox.Instance.GameManager.ChangeScene("Engineering Leech");
    }
}
