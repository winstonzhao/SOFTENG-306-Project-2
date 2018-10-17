using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class to show the help dialog on a hover.
 */
public class HoverHelp : MonoBehaviour {
    public Canvas helpCanvas;

    private void OnMouseOver()
    {
        // set the help dialog to visible
        helpCanvas.enabled = true;
      
    }
}
