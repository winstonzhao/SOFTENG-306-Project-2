using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Hook class for cursor handlers, attach this to every component that you wish the cursor change on
 */
public class CursorStyler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
	public bool enabled = true;
	
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
		if (enabled)
		{
			handler.CursorEnter();
		}
	}

	/**
	 * Mouse exit for GUI elements
	 */
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		if (enabled)
		{
			handler.CursorClear();
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (enabled)
		{
			handler.CursorDrag();
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
			handler.CursorEnter();
		}
	}

	private void OnMouseExit()
	{
		if (!EventSystem.current.IsPointerOverGameObject() && enabled)
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
		if (!EventSystem.current.IsPointerOverGameObject() && enabled)
		{
			handler.CursorDrag();
		}
	}
}
