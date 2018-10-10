using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Click_Dialog : MonoBehaviour {

    public Button start;
    public Canvas helpCanvas;

    void Start()
    {
        start = start.GetComponent<Button>();
        start.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        helpCanvas.enabled = true;
    }
}
