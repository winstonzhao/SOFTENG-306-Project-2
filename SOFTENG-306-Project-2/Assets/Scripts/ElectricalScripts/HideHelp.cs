using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class HideHelp : MonoBehaviour {

    //Make sure to attach these Buttons in the Inspector
    public Button start;
    public Canvas helpDialog;

    private TextMeshProUGUI timerArea;

    // Use this for initialization
    void Start()
    {
        start = start.GetComponent<Button>();
        start.onClick.AddListener(TaskOnClick);

        timerArea = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
    }

    void TaskOnClick()
    {
        helpDialog.enabled = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

}
