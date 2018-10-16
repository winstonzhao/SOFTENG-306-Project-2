using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 
 */ 
public class Click_Dialog : MonoBehaviour {

    public Button start;
    public Canvas[] canvases;

    void Start()
    {
        start = start.GetComponent<Button>();
        start.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        foreach (Canvas canvas in canvases)
        {
            canvas.enabled = !canvas.enabled;
        }
    }
}
