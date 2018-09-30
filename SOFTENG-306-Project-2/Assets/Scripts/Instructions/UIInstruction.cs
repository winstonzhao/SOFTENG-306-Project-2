using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInstruction : MonoBehaviour {

	void Start()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(GetComponentInChildren<Text>().preferredWidth, GetComponentInChildren<Text>().preferredHeight);
        GetComponent<Draggable>().Size = new Vector2(GetComponentInChildren<Text>().preferredWidth, GetComponentInChildren<Text>().preferredHeight);
    }
}
