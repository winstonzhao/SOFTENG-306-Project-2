using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_Tutorial : MonoBehaviour {

    public Button button;
    public Canvas firstCanvas;
    public Canvas secondCanvas;

    void Start()
    {
        button = button.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        firstCanvas.enabled = !firstCanvas.enabled;
        secondCanvas.enabled = !secondCanvas.enabled;
    }
}
