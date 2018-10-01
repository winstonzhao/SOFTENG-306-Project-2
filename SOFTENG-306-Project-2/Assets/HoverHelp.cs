using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverHelp : MonoBehaviour {
    public Canvas helpCanvas;

    private void OnMouseOver()
    {
        helpCanvas.enabled = true;
      
    }
}
