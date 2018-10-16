using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Show_Tutorial : MonoBehaviour {

    public Button button;
    public Canvas firstCanvas;
    public Canvas secondCanvas;

    private TextMeshProUGUI timerArea;

    void Start()
    {
        button = button.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        // find timer text
        timerArea = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
    }

    void TaskOnClick()
    {
        firstCanvas.enabled = !firstCanvas.enabled;
        secondCanvas.enabled = !secondCanvas.enabled;

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
