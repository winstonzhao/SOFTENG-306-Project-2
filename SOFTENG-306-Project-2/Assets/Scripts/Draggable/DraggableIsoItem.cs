using System.Collections.Generic;
using UltimateIsometricToolkit.physics;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;
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
        NE,
        SE,
        SW,
        NW
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

    public IsoDropZone dropZone; // Drop zone linked to initial location, for block factories
    public int Price; // Price of the block for budgeting
    public bool Rotatable = true; // If the block can be rotated

    private bool _mouseInside = false; // Mouse inside the block area
    private bool _dragging = false; // If the block is being dragged
    private bool _moving = false; // If the block is moving
    private static bool _holdingItem = false; // For only dragging one block at a time
    private IsoCollider _currentHitCollider; // Block it is currently hitting
    private readonly List<IsoDropZone> _newDropZones = new List<IsoDropZone>(); // Drop zones the block can be placed in

    private Vector3 _target; // Where the block is being moved to
    private Vector3 _prevMousePos; // Where the mouse was last

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
        get { return homePos; }
        set
        {
            homePos = value;
            if (!_dragging)
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
        if (_dragging && Time.time - lastClickTime > 0.1f)
        {
            // fire a ray cast from the current mouse position to the Isometric dimension system
            var isoRay = Isometric.MouseToIsoRay();
            IsoRaycastHit isoRaycastHit;
            if (IsoPhysics.Raycast(isoRay, out isoRaycastHit))
            {
                // if the ray cast hits something, check which tile is it hitting, trigger entering the drop zone 
                // and exiting the previous drop zone
                var hitCollider = isoRaycastHit.Collider;
                if (hitCollider != _currentHitCollider)
                {
                    // if the ray cast did not hit any tile, the block is moved to an empty space, trigger exiting the  
                    // previous drop zone
                    if (_currentHitCollider != null)
                    {
                        OnIsoTriggerExitDZ(_currentHitCollider);
                    }

                    OnIsoTriggerEnterDZ(hitCollider);
                    _currentHitCollider = hitCollider;
                }
            }
            else
            {
                if (_currentHitCollider != null) // The block is leaving the drop zone
                {
                    OnIsoTriggerExitDZ(_currentHitCollider);
                    _currentHitCollider = null;
                }
            }

            // Move the block visually
            var localMousePos = Input.mousePosition;
            var mousePosWorld =
                Camera.main.ScreenToWorldPoint(new Vector3(localMousePos.x, localMousePos.y,
                    Camera.main.nearClipPlane));
            var diff = mousePosWorld - _prevMousePos;
            GetComponent<IsoTransform>().transform.Translate(diff);
            if (dropZone != null)
            {
                dropZone.OnItemDrag(this);
            }

            _prevMousePos = mousePosWorld;
        }


        if (_moving) // The block is moving to its new drop zone
        {
            transform.position =
                Vector3.Lerp(transform.position, _target, 0.1f); // Transform the position with an animation
            if (Vector3.Distance(transform.position, _target) < 0.01f)
            {
                // Actually move the block there
                GetComponent<IsoTransform>().Position = dropZone.GetComponent<IsoTransform>().Position;
                _moving = false;
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
            _mouseInside = true;
        }
    }

    /**
     * Mouse outside of the block
     */
    void OnMouseLeave()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Block mouse clicks through a canvas
        {
            _mouseInside = false;
        }
    }

    /**
     * Mouse click
     */
    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // Block mouse clicks through a canvas
        if (Time.time - lastClickTime < catchTime) // Double click
        {
            Rotate();
        }
        else // Single click
        {
            if (!_mouseInside || _holdingItem) return; // Mouse not over a block or a block is already being held
            _dragging = true;
            _moving = false;
            _holdingItem = true;
            spriteRenderer.sortingOrder = 1; // Put the block on top
            // Drag the block under the mouse
            Vector3 mousePos = Input.mousePosition;
            Vector3 mousePosWorld =
                Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
            _prevMousePos = mousePosWorld;

            if (dropZone != null)
            {
                dropZone.OnDragStart(this);
            }
        }

        lastClickTime = Time.time;
    }

    /**
     * Click finish
     */
    void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // Block mouse clicks through a canvas
        _dragging = false;
        _holdingItem = false;
        if (_newDropZones.Count > 0 && _newDropZones[0].droppableNames.Contains(this.name)
        ) // Dropped on a new available drop zone
        {
            if (dropZone != null)
            {
                dropZone.OnItemRemove(this);
            }

            _newDropZones[0].OnDrop(this);
            dropZone = _newDropZones[0];
            _newDropZones.Clear();
        }
        else if (_newDropZones.Count > 0 && !_newDropZones[0].droppableNames.Contains(this.name)
        ) // Dropped on a new unavailable drop zone
        {
            _newDropZones[0].OnDragExit(this);
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

        MoveTo(homePos); // Move the block
    }

    /**
     * Block has entered the drop zone
     */
    void OnIsoTriggerEnterDZ(IsoCollider col)
    {
        if (!_dragging) return; // No block being held

        var possibleDropZone = col.GetComponent<IsoDropZone>();
        if (possibleDropZone == null) return; // No drop zone

        foreach (var dropZone in _newDropZones)
        {
            dropZone.OnDragExit(this);
        }

        _newDropZones.Insert(0, possibleDropZone);
        _newDropZones[0].OnDragEnter(this);
    }

    /**
     * Block has exited the drop zone
     */
    void OnIsoTriggerExitDZ(IsoCollider col)
    {
        if (!_dragging) return;

        var possibleDropZone = col.GetComponent<IsoDropZone>();
        if (possibleDropZone == null) return;

        var prevIndex = _newDropZones.IndexOf(possibleDropZone);
        if (prevIndex > -1) // Previous drop zone
        {
            _newDropZones[prevIndex].OnDragExit(this);
            _newDropZones.RemoveAt(prevIndex);

            if (prevIndex == 0 && _newDropZones.Count > 0)
            {
                _newDropZones[0].OnDragEnter(this);
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

        _moving = true;
        _target = newPos;
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
        if (!Rotatable) return;
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