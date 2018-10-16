using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using System;
using UltimateIsometricToolkit.physics;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine.EventSystems;

public class DraggableIsoItem : Draggable
{
    public enum Direction
    {
        NE, SE, SW, NW
    }

    public string name;
    public Direction direction;
    public Sprite NESprite;
    public Sprite SESprite;
    public Sprite SWSprite;
    public Sprite NWSprite;
    public IsoDropZone dropZone;
    public int Price;
    public bool Rotatable = true;


    private bool mouseInside = false;
    private bool dragging = false;
    private bool moving = false;
    private IsoCollider currentHitCollider;

    private static bool holdingItem = false;

    private List<IsoDropZone> newDropZones = new List<IsoDropZone>();

    private Vector3 target;

    Vector3 prevMousePos;
    
    // fields for detecting double clicking
    private static float lastClickTime = 0;
    private static float catchTime = 0.3f;

    public Vector3 homePos;
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
        SetDirection(this.direction);
        homePos = GetComponentInParent<Transform>().position;
    }

    public void SetDropZone(IsoDropZone list)
    {
        this.dropZone = list;
    }

    public override void SetDropZone(IDropZone list)
    {
        var isoDropZone = list as IsoDropZone;
        if (isoDropZone != null)
        {
            SetDropZone(isoDropZone);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // give a small gap before dragging, preventing that when user clicks on the DraggableIsoItem, the mouse is 
        // also firing at the IsoDropZone besides it, causing the block to be shifted to the drag zone beside
        if (dragging && Time.time - lastClickTime > 0.1f) 
        {
            // fire a ray cast from the current mouse position to the Isometric dimension system
            var isoRay = Isometric.MouseToIsoRay();
            IsoRaycastHit isoRaycastHit;
            if (IsoPhysics.Raycast(isoRay, out isoRaycastHit))
            {
                // if the ray cast hits something, check which tile is it hitting, trigger entering the drop zone 
                // and exiting the previous drop zone
                IsoCollider hitCollider = isoRaycastHit.Collider;
                if (hitCollider != currentHitCollider)
                {
                    // if the ray cast did not hit any tile, the block is moved to an empty space, trigger exiting the  
                    // previous drop zone
                    if (currentHitCollider != null)
                    {
                        OnIsoTriggerExitDZ(currentHitCollider);
                    }
                    OnIsoTriggerEnterDZ(hitCollider);
                    currentHitCollider = hitCollider;
                }
            }
            else
            {
                if (currentHitCollider != null)
                {
                    OnIsoTriggerExitDZ(currentHitCollider);
                    currentHitCollider = null;
                }
            }
            
            Vector3 localMousePos = Input.mousePosition;
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(localMousePos.x, localMousePos.y, Camera.main.nearClipPlane));

            Vector3 diff = mousePosWorld - prevMousePos;
            GetComponent<IsoTransform>().transform.Translate(diff);

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
                GetComponent<IsoTransform>().Position = dropZone.GetComponent<IsoTransform>().Position;
                moving = false;
            }
        }
    }

    void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
        {
            mouseInside = true;
        }
    }

    void OnMouseLeave()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
        {
            mouseInside = false;
        }
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
        {
            if (Time.time - lastClickTime < catchTime) // double click
            {
                Rotate();
            }
            else // single click
            {
                if (!mouseInside || holdingItem) return;
                dragging = true;
                moving = false;
                holdingItem = true;
                GetComponent<SpriteRenderer>().sortingOrder = 1;
                Vector3 mousePos = Input.mousePosition;
                Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
                prevMousePos = mousePosWorld;

                if (dropZone != null)
                {
                    dropZone.OnDragStart(this);
                }
            }
            lastClickTime = Time.time;
        }
    }

    void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
        {
            dragging = false;
            holdingItem = false;
            if (newDropZones.Count > 0 && newDropZones[0].droppableNames.Contains(this.name)
            ) // dropped on a new available drop zone
            {
                if (dropZone != null)
                {
                    dropZone.OnItemRemove(this);
                }

                newDropZones[0].OnDrop(this);
                dropZone = newDropZones[0];
                newDropZones.Clear();
            }
            else if (newDropZones.Count > 0 && !newDropZones[0].droppableNames.Contains(this.name)
            ) // dropped on a new unavailable drop zone
            {
                newDropZones[0].OnDragExit(this);
                if (dropZone != null)
                {
                    dropZone.OnDragFinish(this);
                }
            }
            else // did not drop on any new drop zone
            {
                if (dropZone != null)
                {
                    dropZone.OnDragFinish(this);
                }
            }
            MoveTo(homePos);
        }
    }

    void OnIsoTriggerEnterDZ(IsoCollider col)
         {
             if (!dragging) return;
     
             var possibleDropZone = col.GetComponent<IsoDropZone>();
             if (possibleDropZone == null) return;
     
             foreach (var dropZone in newDropZones)
             {
                 dropZone.OnDragExit(this);
             }
     
             newDropZones.Insert(0, possibleDropZone);
             newDropZones[0].OnDragEnter(this);
         }

    void OnIsoTriggerExitDZ(IsoCollider col)
    {
        if (!dragging) return;

        var possibleDropZone = col.GetComponent<IsoDropZone>();
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

        moving = true;
        target = newPos;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    public void SetDirection(Direction dir)
    {
        this.direction = dir;

        switch (dir)
        {
            case Direction.NE:
                SetSprite(NESprite);
                break;
            case Direction.SE:
                SetSprite(SESprite);
                break;
            case Direction.SW:
                SetSprite(SWSprite);
                break;
            case Direction.NW:
                SetSprite(NWSprite);
                break;

        }
    }

    private void SetSprite(Sprite sprite)
    {
        GetComponentInParent<SpriteRenderer>().sprite = sprite;
    }

    public void Rotate()
    {
        if (Rotatable)
        {
            switch (this.direction)
            {
                case Direction.NE:
                    SetDirection(Direction.SE);
                    break;
                case Direction.SE:
                    SetDirection(Direction.SW);
                    break;
                case Direction.SW:
                    SetDirection(Direction.NW);
                    break;
                case Direction.NW:
                    SetDirection(Direction.NE);
                    break;

            }
        }
        
    }
}
