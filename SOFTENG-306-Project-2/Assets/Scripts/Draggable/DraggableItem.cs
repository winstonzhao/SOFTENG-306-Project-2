using System.Collections;
using System.Collections.Generic;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using System;

public class DraggableItem : Draggable
{

    private bool mouseInside = false;
    private bool dragging = false;
    private bool moving = false;

    [NonSerialized]
    public IDropZone dropZone;

    private List<IDropZone> newDropZones = new List<IDropZone>();

    private Vector3 target;

    Vector3 prevMousePos;

    private List<DraggableItem> connectedItems = new List<DraggableItem>();

    public delegate void OnDropZoneChangedDelegate(IDropZone dropZone);

    public OnDropZoneChangedDelegate OnDropZoneChanged;

    private float seconds = 0.3f;
    private float t = 0;

    private Vector3 homePos;
    public override Vector3 HomePos
    {
        get
        {
            return homePos;
        }
        set
        {
            homePos = value;
            if (!dragging)
            {
                MoveTo(value);
            }
        }
    }

    public override Vector2 Size { get; set; }

    /// <summary>
    /// Changes the current drop zone to the given drop zone
    /// </summary>
    public override void SetDropZone(IDropZone newDropZone)
    {
        dropZone = newDropZone;
    }

    /// <summary>
    /// Add an item that will move drop zones and destroy with this item
    /// </summary>
    public void AddConnectedItem(DraggableItem item)
    {
        connectedItems.Add(item);
    }

    void FixedUpdate()
    {
        // Handle dragging
        if (dragging)
        {
            Vector3 localMousePos = Input.mousePosition;
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(localMousePos.x, localMousePos.y, Camera.main.nearClipPlane));

            Vector3 diff = mousePosWorld - prevMousePos;

            var isoTransform = GetComponent<IsoTransform>();
            if (isoTransform != null) 
            {
                isoTransform.transform.Translate(diff);
            }
            else
            {
                transform.Translate(diff);
            }

            // Notify the current drop zone of movement
            if (dropZone != null) 
            {
                dropZone.OnItemDrag(this);
            }

            prevMousePos = mousePosWorld;
        }

        if (moving)
        {
            // Move back to home position

            var tdiff = t;
            t += Time.deltaTime / seconds;
            if (t >= 1)
            {
                moving = false;

                // Reset t so position is not overshot
                t = 1;
            }
            tdiff = t - tdiff;

            transform.position += tdiff * target;
        }
    }

    void OnMouseEnter()
    {
        mouseInside = true;
    }

    void OnMouseLeave()
    {
        mouseInside = false;
    }

    void OnMouseDown()
    {
        if (!mouseInside) return;
        dragging = true;
        moving = false;

        // Move infront of other objects
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.sortingOrder = 1;

        // Calculate mouse position
        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
        prevMousePos = mousePosWorld;

        if (dropZone != null)
        {
            // Notify current drop zone of drag
            dropZone.OnDragStart(this);
        }
    }

    void OnMouseUp()
    {
        dragging = false;

        // Check if touching any drop zones (that are not the current one)
        if (newDropZones.Count > 0 && newDropZones[0] != dropZone)
        {
            if (dropZone != null) 
            {
                // Remove this and connected items from current drop zone
                dropZone.OnItemRemove(this);
                connectedItems.ForEach(i => dropZone.OnItemRemove(i));
            }

            // Set new drop zone
            OnDrop(newDropZones[0]);
            newDropZones.Clear();
        }
        else
        {
            // Notify current drop zone there has been no change
            if (dropZone != null) 
            {
                dropZone.OnDragFinish(this);
            }
        }

        MoveTo(homePos);
    }

    private void OnDrop(IDropZone newDropZone)
    {
        newDropZone.OnDrop(this);
        if (OnDropZoneChanged != null) OnDropZoneChanged(newDropZone);

        foreach (var item in connectedItems)
        {
            newDropZone.OnDrop(item);
            item.dropZone = newDropZone;
            item.MoveTo(item.HomePos);
        }
        dropZone = newDropZone;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!dragging) return;
    
        var possibleDropZone = col.GetComponent<IDropZone>();
        if (possibleDropZone == null || !possibleDropZone.CanDrop(this)) return;

        // Notify current potential drop zones they aren't first choice
        foreach(var dropZone in newDropZones)
        {
            dropZone.OnDragExit(this);
        }

        // Add new drop zone as first choice
        newDropZones.Insert(0, possibleDropZone);
        newDropZones[0].OnDragEnter(this);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!dragging) return;

        var possibleDropZone = col.GetComponent<IDropZone>();
        if (possibleDropZone == null) return;

        // Notify drop zone that item has been move outside
        var prevIndex = newDropZones.IndexOf(possibleDropZone);
        if (prevIndex > -1)
        {
            newDropZones[prevIndex].OnDragExit(this);
            newDropZones.RemoveAt(prevIndex);

            // Set a new first choice drop zone and notify it
            if (prevIndex == 0 && newDropZones.Count > 0)
            {
                newDropZones[0].OnDragEnter(this);
            }
        }
        else if (possibleDropZone == dropZone)
        {
            dropZone.OnDragExit(this);
        }

    }


   public void MoveTo(Vector3 newPos)
    {
        if (dropZone == null) return;

        // Initialise variables for movement
        t = 0;
        moving = true;
        target = newPos - transform.position;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.sortingOrder = 0;
    }
}
