using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if UNITYEDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class IsoObject : MonoBehaviour, IComparable<IsoObject>
{
    //Position in virtual 3d space, not the Isometric projection (which is a Vector2 and stored in transform.position)
    [SerializeField] private Vector3 position;

    [SerializeField] private Vector3 size = new Vector3(1, 1, 1);

    public bool displayBounds = true;

    private Bounds bounds = new Bounds();

    private SpriteRenderer renderer;

    private List<IsoObject> behindObjects = new List<IsoObject>();

    [Tooltip("Will use this object to compute z-order")]
    public IsoObject Ground;

    [Tooltip("Use this to offset the object slightly in front or behind the Target object")]
    public int TargetOffset = 0;

    public List<IsoObject> BehindObjects
    {
        get { return behindObjects; }
        set { behindObjects = value; }
    }

    private bool visited = false;

    public bool Visited
    {
        get { return visited; }
        set { visited = value; }
    }

    public Bounds Bounds
    {
        get { return bounds; }
        private set
        {
            bounds = value;
            position = bounds.center;
            size = bounds.extents * 2;
        }
    }

    public Vector3 Size
    {
        get { return size; }
        set
        {
            size = value;
            bounds.extents = value / 2;
        }
    }

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        bounds.center = position;
        bounds.extents = size / 2;
    }

    void Start()
    {
        updateIsoProjection();
    }

    public void Update()
    {
        updateIsoProjection();
    }

    //the position of the object, not the isometric projection
    public Vector3 Position
    {
        get { return position; }
        set
        {
            position = value;
            bounds.center = value;
            updateIsoProjection();
        }
    }

    /// <summary>
    /// updates the isometric projection of this isoObject
    /// </summary>
    protected void updateIsoProjection()
    {
        transform.position = isoProjection(Position);
    }

    /// <summary>
    /// current depth/sortingOrder. Wrapper for renderer.sortingOrder.
    /// </summary>
    public int Depth
    {
        get
        {
            if (renderer == null)
            {
                renderer = GetComponent<SpriteRenderer>();
            }

            return renderer.sortingOrder;
        }
        set
        {
            if (renderer == null)
            {
                renderer = GetComponent<SpriteRenderer>();
            }

            renderer.sortingOrder = value;
        }
    }

    /// <summary>
    /// Isometric Projection of an object in 3d Space. Depth-Sorting is always depends on the projection function
    /// </summary>
    /// <param name="pt"></param> The Object in virtual 3d space
    /// <returns></returns>
    public Vector2 isoProjection(Vector3 pt)
    {
        Vector2 vec = new Vector2(0, 0);
        vec.x = (pt.x - pt.y);
        vec.y = (pt.x + pt.y) / 2;
        vec.y += pt.z;
        return vec;
    }

    /// <summary>
    /// compare function used for sorting. Depends on isometric projection function
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(IsoObject other)
    {
        var intersectionTolerence = .00f;
        if (other == this)
            return 0;
        //we are in front of other
        if (other.bounds.min.z * (1 + intersectionTolerence) < bounds.max.z &&
            other.bounds.max.x > bounds.min.x * (1 + intersectionTolerence) &&
            other.bounds.max.y > bounds.min.y * (1 + intersectionTolerence))
        {
            //Debug.Log(this.name + bounds + " isBefore " + other.name + other.bounds);
            return 1;
        }
        //we are behind other
        else
        {
            //Debug.Log(this.name + bounds + " isBehind " + other.name + other.bounds);
            return -1;
        }
    }
}
