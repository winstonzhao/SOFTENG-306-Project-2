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
	public bool enabled = true;
	
	private CursorHandler _handler;

	/**
	 * Set the default cursor handler
	 */
	void Awake()
	{
		_handler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CursorHandler>();
	}

	
	/**
	 * Mouse enter for GUI elements
	 */
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		if (enabled)
		{
			_handler.CursorEnter();
		}
	}

	/**
	 * Mouse exit for GUI elements
	 */
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		if (enabled)
		{
			_handler.CursorClear();
		}
	}

	
	/**
	 * =========================================
	 * For GameObjects
	 * =========================================
	 */
	
	
	void OnMouseEnter()
	{
		if (!EventSystem.current.IsPointerOverGameObject() && enabled) // Block mouse clicks through a canvas
		{
			_handler.CursorEnter();
		}
	}

	private void OnMouseExit()
	{
		if (!EventSystem.current.IsPointerOverGameObject() && enabled)
		{
			_handler.CursorClear();
		}
	}

	private void OnMouseUp()
	{
		_handler.CursorClear();
	}

	private void OnMouseDrag()
	{
		if (!EventSystem.current.IsPointerOverGameObject() && enabled)
		{
			_handler.CursorDrag();
		}
	}

}
