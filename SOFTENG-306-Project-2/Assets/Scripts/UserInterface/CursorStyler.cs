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

    private CursorHandler _handler;

    /**
     * Set the default cursor handler
     */
    private void Awake()
    {
        _handler = FindObjectOfType<CursorHandler>();
    }

    private void OnDestroy()
    {
        // Reset the cursor afterwards
        _handler.CursorClear();
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

    public void OnDrag(PointerEventData eventData)
    {
        if (enabled)
        {
            _handler.CursorDrag();
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
