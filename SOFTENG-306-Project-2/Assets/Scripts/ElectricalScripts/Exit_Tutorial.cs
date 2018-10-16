using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Class to exit the tutorial and return to the game
 */
public class Exit_Tutorial : MonoBehaviour {

    public Button exitButton;
    public Canvas[] canvases;

    private TextMeshProUGUI timerArea;

    void Start()
    {
        // set a listener on the exit button
        exitButton = exitButton.GetComponent<Button>();
        exitButton.onClick.AddListener(TaskOnClick);

        // find the timer text
        timerArea = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
    }

    void TaskOnClick()
    {
        // set each canvas as invisible 
        foreach (Canvas canvas in canvases)
        {
            canvas.enabled = false;
        }

        // change colour of timer text
        timerArea.color = Color.black;
    }
}
