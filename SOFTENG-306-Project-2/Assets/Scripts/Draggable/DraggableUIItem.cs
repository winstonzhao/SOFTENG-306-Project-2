using System.Collections;
using System.Collections.Generic;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using System;
using UnityEngine.EventSystems;

public class DraggableUIItem : Draggable, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    private bool mouseInside = false;
    private bool dragging = false;
    private bool moving = false;

    [NonSerialized]
    public IDropZone dropZone;

    private List<IDropZone> newDropZones = new List<IDropZone>();

    private Vector3 target;

    Vector3 prevMousePos;

    private List<DraggableUIItem> connectedItems = new List<DraggableUIItem>();

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

    // Use this for initialization
    void Start()
    {

    }

    private void OnDestroy()
    {
        foreach (var item in connectedItems)
        {
            if (item != null)
            {
                if (item.dropZone != null) item.dropZone.OnItemRemove(item);
                Destroy(item.gameObject);
            }
        }
    }

    public override void SetDropZone(IDropZone newDropZone)
    {
        dropZone = newDropZone;
    }

    public void AddConnectedItem(DraggableUIItem item)
    {
        connectedItems.Add(item);
    }

    // Update is called once per frame
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

            if (dropZone != null) 
            {
                dropZone.OnItemDrag(this);
            }

            prevMousePos = mousePosWorld;

            if (newDropZones.Count > 0)
            {
                newDropZones[0].OnDragEnter(this);
            }
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

            (transform as RectTransform).anchoredPosition += new Vector2((tdiff * target).x, (tdiff * target).y);
        }
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
        
        foreach(var dropZone in newDropZones)
        {
            dropZone.OnDragExit(this);
        }
    
        newDropZones.Insert(0, possibleDropZone);
        newDropZones[0].OnDragEnter(this);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!dragging) return;

        var possibleDropZone = col.GetComponent<IDropZone>();
        if (possibleDropZone == null) return;
    
        var prevIndex = newDropZones.IndexOf(possibleDropZone);
        if (prevIndex > -1)
        {
            newDropZones[prevIndex].OnDragExit(this);
            newDropZones.RemoveAt(prevIndex);

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

        t = 0;
        moving = true;
        target = new Vector2(newPos.x, newPos.y) - (transform as RectTransform).anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseInside = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!mouseInside) return;
        dragging = true;
        moving = false;

        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
        prevMousePos = mousePosWorld;

        if (dropZone != null)
        {
            dropZone.OnDragStart(this);
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        if (newDropZones.Count > 0 && newDropZones[0] != dropZone)
        {
            if (dropZone != null)
            {
                dropZone.OnItemRemove(this);
                connectedItems.ForEach(i => dropZone.OnItemRemove(i));
            }

            OnDrop(newDropZones[0]);
            newDropZones.Clear();
        }
        else
        {
            if (dropZone != null)
            {
                dropZone.OnDragFinish(this);
            }
        }
        MoveTo(homePos);
    }
}
