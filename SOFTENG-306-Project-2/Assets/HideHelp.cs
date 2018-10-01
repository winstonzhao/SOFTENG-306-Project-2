using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideHelp : MonoBehaviour {

    //Make sure to attach these Buttons in the Inspector
    public Button start;
    public Canvas helpDialog;

    // Use this for initialization
    void Start()
    {
        start = start.GetComponent<Button>();

        start.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        helpDialog.enabled = false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
