using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using System;
using UltimateIsometricToolkit.physics;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine.EventSystems;

/**
 * Draggable iso blocks used in Civil Mini-game
 */
public class DraggableIsoItem : Draggable
{
    /**
     * Direction blocks can be oriented in
     */
    public enum Direction
    {
        NE, SE, SW, NW
    }

    public string name; // Name of the block
    public Direction direction; // Direction the block is facing
    /**
     * Direction Sprites
     */
    public Sprite NESprite;
    public Sprite SESprite;
    public Sprite SWSprite;
    public Sprite NWSprite;
    
    public IsoDropZone dropZone;  // Drop zone linked to initial location, for block factories
    public int Price;  // Price of the block for budgeting
    public bool Rotatable = true;  // If the block can be rotated

    private bool mouseInside = false; // Mouse inside the block area
    private bool dragging = false; // If the block is being dragged
    private bool moving = false; // If the block is moving
    private static bool holdingItem = false;  // For only dragging one block at a time
    private IsoCollider currentHitCollider; // Block it is currently hitting
    private List<IsoDropZone> newDropZones = new List<IsoDropZone>(); // Drop zones the block can be placed in

    private Vector3 target; // Where the block is being moved to
    Vector3 prevMousePos;  // Where the mouse was last
    
    // Fields for detecting double clicking
    private static float lastClickTime = 0;
    private static float catchTime = 0.3f; // If another click is registered within this time, it is a double click

    
    /**
    * Get component fields
    */
    private SpriteRenderer spriteRenderer;
    
    /**
     * Position of the block to return to
     */
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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /**
     * Set the drop zone this block belongs too, executor
     */
    public void SetDropZone(IsoDropZone list)
    {
        this.dropZone = list;
    }

    /**
     * Set the drop zone this block belongs too, handler
     */
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
                if (currentHitCollider != null) // The block is leaving the drop zone
                {
                    OnIsoTriggerExitDZ(currentHitCollider);
                    currentHitCollider = null;
                }
            }
            
            // Move the block visually
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


        if (moving)  // The block is moving to its new drop zone
        {
            transform.position = Vector3.Lerp(transform.position, target, 0.1f); // Transform the position with an animation
            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                // Actually move the block there
                GetComponent<IsoTransform>().Position = dropZone.GetComponent<IsoTransform>().Position;
                moving = false;
            }
        }
    }

    /**
     * Mouse on top of the block
     */
    void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
        {
            mouseInside = true;
        }
    }

    /**
     * Mouse outside of the block
     */
    void OnMouseLeave()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
        {
            mouseInside = false;
        }
    }

    /**
     * Mouse click
     */
    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
        {
            if (Time.time - lastClickTime < catchTime) // Double click
            {
                Rotate();
            }
            else // Single click
            {
                if (!mouseInside || holdingItem) return; // Mouse not over a block or a block is already being held
                dragging = true;
                moving = false;
                holdingItem = true;
                spriteRenderer.sortingOrder = 1;  // Put the block on top
                // Drag the block under the mouse
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

    /**
     * Click finish
     */
    void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
        {
            dragging = false;
            holdingItem = false;
            if (newDropZones.Count > 0 && newDropZones[0].droppableNames.Contains(this.name)
            ) // Dropped on a new available drop zone
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
            ) // Dropped on a new unavailable drop zone
            {
                newDropZones[0].OnDragExit(this);
                if (dropZone != null)
                {
                    dropZone.OnDragFinish(this);
                }
            }
            else // Did not drop on any new drop zone
            {
                if (dropZone != null)
                {
                    dropZone.OnDragFinish(this);
                }
            }
            MoveTo(homePos);  // Move the block
        }
    }

    /**
     * Block has entered the drop zone
     */
    void OnIsoTriggerEnterDZ(IsoCollider col)
         {
             if (!dragging) return;  // No block being held
     
             var possibleDropZone = col.GetComponent<IsoDropZone>();
             if (possibleDropZone == null) return;  // No drop zone
     
             foreach (var dropZone in newDropZones)
             {
                 dropZone.OnDragExit(this);
             }
     
             newDropZones.Insert(0, possibleDropZone);
             newDropZones[0].OnDragEnter(this);
         }

    /**
     * Block has exited the drop zone
     */
    void OnIsoTriggerExitDZ(IsoCollider col)
    {
        if (!dragging) return;

        var possibleDropZone = col.GetComponent<IsoDropZone>();
        if (possibleDropZone == null) return;

        var prevIndex = newDropZones.IndexOf(possibleDropZone);
        if (prevIndex > -1) // Previous drop zone
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

    /**
     * Move block to its new location
     */
    public void MoveTo(Vector3 newPos)
    {
        if (dropZone == null) return;

        moving = true;
        target = newPos;
        spriteRenderer.sortingOrder = 0;
    }

    /**
     * Set the direction the block is facing by changing to the corresponding sprite
     */
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

    /**
     * Change the sprite for the block
     */
    private void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    /**
     * Rotate the block clockwise
     */
    public void Rotate()
    {
        if (Rotatable) // Block can be rotated
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
