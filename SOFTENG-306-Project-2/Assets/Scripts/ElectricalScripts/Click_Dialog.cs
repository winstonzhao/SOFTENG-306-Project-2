using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * A script to control the transitions between scenes
 * and exiting the minigame
 */ 
public class Click_Dialog : MonoBehaviour {

    public Button start;
    public Canvas[] canvases;

    void Start()
    {
        // get the button that is clicked
        start = start.GetComponent<Button>();
        start.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        // disable all canvases specified when button is clicked
        foreach (Canvas canvas in canvases)
        {
            canvas.enabled = !canvas.enabled;
        }
    }
}
