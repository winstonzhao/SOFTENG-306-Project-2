using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour {

	public UnityEvent onClick;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown()
	{
		if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
		{

			onClick.Invoke();
		}
	}
}
