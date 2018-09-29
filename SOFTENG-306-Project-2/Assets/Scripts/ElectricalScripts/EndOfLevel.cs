using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour {

    private Canvas backCanvas;

    // Use this for initialization
    void Start () {
        backCanvas = GetComponent<Canvas>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backCanvas.enabled = !backCanvas.enabled;
        }
    }
}
