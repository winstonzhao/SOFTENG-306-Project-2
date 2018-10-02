using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpDialog : MonoBehaviour {

    public Canvas helpCanvas;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) {
            helpCanvas.enabled = true;
        }
    }
}
