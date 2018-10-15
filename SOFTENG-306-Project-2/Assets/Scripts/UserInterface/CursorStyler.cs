using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Hook class for cursor handlers, attach this to every component that you wish the cursor change on
 */
public class CursorStyler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private CursorHandler handler;

	/**
	 * Set the default cursor handler
	 */
	void Awake()
	{
		handler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CursorHandler>();
	}

	
	/**
	 * Mouse enter for GUI elements
	 */
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		handler.CursorEnter();
	}

	/**
	 * Mouse exit for GUI elements
	 */
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		handler.CursorClear();
	}

	
	/**
	 * =========================================
	 * For GameObjects
	 * =========================================
	 */
	
	
	void OnMouseEnter()
	{
		if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
		{
			handler.CursorEnter();
		}
	}

	private void OnMouseExit()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			handler.CursorClear();
		}
	}

	private void OnMouseUp()
	{

		handler.CursorClear();

	}

	private void OnMouseDrag()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			handler.CursorDrag();
		}
	}

}
