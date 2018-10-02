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

    public GameObject block1;
    public GameObject block2;
    public GameObject left;
    public GameObject right;
    public GameObject tile;

    Vector3 prevMousePos;

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

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;


        block1 = GameObject.Find("Block1");
        block2 = GameObject.Find("Block2");
        left = GameObject.Find("LeftTile");
        right = GameObject.Find("RightTile");
        tile = GameObject.Find("Tile");
    }

    public override void SetDropZone(IDropZone list)
    {
        this.dropZone = list;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dragging)
        {
            Vector3 localMousePos = Input.mousePosition;
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(localMousePos.x, localMousePos.y, Camera.main.nearClipPlane));

            Vector3 diff = mousePosWorld - prevMousePos;
            transform.Translate(diff);

            if (dropZone != null)
            {
                dropZone.OnItemDrag(this);
            }

            prevMousePos = mousePosWorld;
        }

        if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, target, 0.1f);

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

    void OnMouseDown()
    {
        moveTimer += Time.deltaTime;

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

    void OnMouseUp()
    {
        dragging = false;

        if (inDropZone)
        {
            MoveTo(homePos);
        }
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
        MoveTo(homePos);
    }

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
