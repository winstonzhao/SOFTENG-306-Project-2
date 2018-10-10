using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exit_Tutorial : MonoBehaviour {

    public Button exit;
    public Canvas backgroundCanvas;
    public Canvas screen1;
    public Canvas screen2;
    public Canvas screen3;
    public Canvas screen4;
    public Canvas screen5;

    void Start()
    {
        exit = exit.GetComponent<Button>();
        exit.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        backgroundCanvas.enabled = false;
        screen1.enabled = false;
        screen2.enabled = false;
        screen3.enabled = false;
        screen4.enabled = false;
        screen5.enabled = false;
    }
}
