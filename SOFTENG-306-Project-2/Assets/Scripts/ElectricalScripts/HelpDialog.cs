using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class to show the help dialog on right click
 */
public class HelpDialog : MonoBehaviour {

    public Canvas helpCanvas;

    private void OnMouseOver()
    {
        // show the canvas on right click
        if (Input.GetMouseButtonDown(1)) {
            helpCanvas.enabled = true;
        }
    }
}
