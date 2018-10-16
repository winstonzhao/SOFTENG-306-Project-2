using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/*
 * Class to hide the help dialog 
 */
public class HideHelp : MonoBehaviour {

    //Make sure to attach these Buttons in the Inspector
    public Button start;
    public Canvas helpDialog;
    private TextMeshProUGUI timerArea;

    void Start()
    {
        start = start.GetComponent<Button>();
        start.onClick.AddListener(TaskOnClick);

        timerArea = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
    }

    void TaskOnClick()
    {
        // set the help dialog to invisible
        helpDialog.enabled = false;
    }

}
