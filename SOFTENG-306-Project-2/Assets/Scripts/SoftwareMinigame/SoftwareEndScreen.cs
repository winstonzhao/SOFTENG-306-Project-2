using System;
using Game;
using Game.Hiscores;
using UnityEngine;

public class SoftwareEndScreen : MonoBehaviour
{
    public Canvas InstructionCanvas;
    public DraggableScrollList scrollList;

    private Transform slides;
    
    // Use this for initialization
    void Start()
    {
        slides = transform.Find("ExitSlide");
        Open();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Open()
    {
        gameObject.SetActive(true);
        InstructionCanvas.gameObject.SetActive(false);
    }

    public void Close()
    {
        InstructionCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    // Used this method to link back to lobby
    public void EndMiniGame()
    {
        var maxScore = 100;
        var instructionCount = scrollList.listItems.Count;
        var score = new Score()
        {
            CreatedAt = DateTime.Now,
            Minigame =  Minigames.Software,
            Value = maxScore - instructionCount,

        };
        Toolbox.Instance.Hiscores.Add(score);
    }
}