using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/*
 * Class to show the tutorial and set the timer to inactive
 */
public class Show_Tutorial : MonoBehaviour {

    public Button button;
    public Canvas[] canvases;
    private TextMeshProUGUI timerArea;

    void Start()
    {
        // set a listener on the exit button
        button = button.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        // find timer text
        timerArea = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
    }

    void TaskOnClick()
    {
        // set each canvas as visible
        foreach (Canvas canvas in canvases)
        {
            canvas.enabled = !canvas.enabled;
        }

        // change the colour of timer text to black to activate timer if player exits tutorial
        if ((EventSystem.current.currentSelectedGameObject.name == "Exit_Button") ||
            (button.GetComponentInChildren<Text>().text == "play") ||
            (button.GetComponentInChildren<Text>().text == "Begin")) {
            timerArea.color = Color.black;
        } else {
            timerArea.color = Color.gray;
        }
    }
}
