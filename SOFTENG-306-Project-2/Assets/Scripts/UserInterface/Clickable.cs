using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour {

	public UnityEvent onClick;

	void OnMouseDown()
	{
		if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
		{
			onClick.Invoke();
		}
	}
}
