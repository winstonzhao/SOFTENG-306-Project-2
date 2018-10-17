using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableItemReturn : Draggable
{

    private bool mouseInside = false;
    private bool dragging = false;
    private bool moving = false;
    private bool inDropZone = false;

    private IDropZone dropZone;

    private List<IDropZone> newDropZones = new List<IDropZone>();

    private Vector3 target;
    private Vector3 startPosition, endPosition;
    private float moveSeconds = 5;
    private float moveTimer = 0;

    Vector3 prevMousePos;
    private Vector3 homePos;

    // get and set the home position
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

    // get and set the size
    public override Vector2 Size { get; set; }

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
    }

    public override void SetDropZone(IDropZone list)
    {
        this.dropZone = list;
    }

    /*
     * Update method called once per frame
     */
    void FixedUpdate()
    {
        // if the player is dragging the logic gate, log the movement of the player
        if (dragging)
        {
            // get mouse position
            Vector3 localMousePos = Input.mousePosition;
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(localMousePos.x, localMousePos.y, Camera.main.nearClipPlane));
            Vector3 diff = mousePosWorld - prevMousePos;
            transform.Translate(diff);

            // if the dropzone exits, log the draggable item as dragging
            if (dropZone != null)
            {
                dropZone.OnItemDrag(this);
            }
            prevMousePos = mousePosWorld;
        }

        // if the player is moving their mouse
        if (moving)
        {
            // transform the item position
            transform.position = Vector3.Lerp(transform.position, target, 0.1f);
            // if the distance moved is very small, log the player as stopped moving
            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                moving = false;
            }
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

    /*
     * When the player holds their mouse down, tracks the movement of the gate
     */
    void OnMouseDown()
    {
        // track how long the items has been moving
        moveTimer += Time.deltaTime;
        if (!mouseInside) return;

        // uodate the player mouse position as they move
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

    void OnMouseUp()
    {
        dragging = false;

        // if the item is in a dropzone, drop the item into it
        if (inDropZone)
        {
            MoveTo(homePos);
        }
        // otherwise, move the item back to its original position linearly
        else
        {
            endPosition = transform.position;
            float ratio = moveTimer / moveSeconds;
            transform.position = Vector3.Lerp(startPosition, endPosition, ratio);
            moveTimer = 0;
        }

        if (newDropZones.Count > 0)
        {
            if (dropZone != null)
            {
                dropZone.OnItemRemove(this);
            }

            newDropZones[0].OnDrop(this);
            dropZone = newDropZones[0];
            newDropZones.Clear();
        }
        else
        {
            if (dropZone != null)
            {
                dropZone.OnDragFinish(this);
            }
        }
        // move the player back to home position
        MoveTo(homePos);
    }

    /*
     * Controller for the 2D collider enter event
     */
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!dragging) return;
        inDropZone = true;
        var possibleDropZone = col.GetComponent<IDropZone>();
        if (possibleDropZone == null) return;

        foreach (var dropZone in newDropZones)
        {
            dropZone.OnDragExit(this);
        }

        newDropZones.Insert(0, possibleDropZone);
        newDropZones[0].OnDragEnter(this);
    }

    /*
     * Controller for the 2D collider exit event
     */
    void OnTriggerExit2D(Collider2D col)
    {
        if (!dragging) return;
        inDropZone = false;
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

    void MoveTo(Vector3 newPos)
    {
        if (dropZone == null) return;

        moving = true;
        target = newPos;
    }
}
