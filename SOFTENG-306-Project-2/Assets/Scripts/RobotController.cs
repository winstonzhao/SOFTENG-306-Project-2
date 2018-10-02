using Ultimate_Isometric_Toolkit.Scripts.Core;
using UltimateIsometricToolkit.physics;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(IsoTransform))]
[RequireComponent(typeof(IsoBoxCollider))]
[RequireComponent(typeof(IsoRigidbody))]
public class RobotController : MonoBehaviour
{

    private IsoTransform isoTransform;
    private SoftwareLevelGenerator generator;

    private int X = 1;
    private int Z = 1;
    private bool hasElement = false;
    private GameObject carrying;

    private static string NO_ELEMENT = "software_minigame/Sprites/robot1";
    private static string HAS_ELEMENT = "software_minigame/Sprites/robot2";

    public enum Direction
    {
        TopRight = 3,
        BottomRight = 1,
        BottomLeft = 2,
        TopLeft = 0
    }

    // Use this for initialization
    void Start()
    {
        isoTransform = this.GetOrAddComponent<IsoTransform>();
        generator = gameObject.GetComponentInParent(typeof(SoftwareLevelGenerator)) as SoftwareLevelGenerator;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool Move(int x, int z)
    {
        for (int i = x; i != 0;)
        {
            if (i < 0)
            {
                if (MoveBL())
                {
                    i++;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (MoveTR())
                {
                    i--;
                }
                else
                {
                    return false;
                }
            }
        }

        for (int i = z; i != 0;)
        {
            if (i < 0)
            {
                if (MoveBR())
                {
                    i++;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (MoveTL())
                {
                    i--;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool MoveBL()
    {
        isoTransform.Translate(new Vector3(-1, 0, 0));
        X--;

        if (generator.GetMapLayout(X, Z) != SoftwareLevelGenerator.Layout.EMPTY)
        {
            Debug.Log("COLLIDE");
            isoTransform.Translate(new Vector3(1, 0, 0));
            X++;
            Debug.Log("To: x: " + X + " z: " + Z);
            return false;
        }
        Debug.Log("To: x: " + X + " z: " + Z);
        return true;
    }

    public bool MoveTL()
    {
        isoTransform.Translate(new Vector3(0, 0, 1));
        Z++;

        if (generator.GetMapLayout(X, Z) != SoftwareLevelGenerator.Layout.EMPTY)
        {
            Debug.Log("COLLIDE");
            isoTransform.Translate(new Vector3(0, 0, -1));
            Z--;
            Debug.Log("To: x: " + X + " z: " + Z);
            return false;
        }
        Debug.Log("To: x: " + X + " z: " + Z);
        return true;
    }

    public bool MoveBR()
    {
        isoTransform.Translate(new Vector3(0, 0, -1));
        Z--;

        if (generator.GetMapLayout(X, Z) != SoftwareLevelGenerator.Layout.EMPTY)
        {
            Debug.Log("COLLIDE");
            isoTransform.Translate(new Vector3(0, 0, 1));
            Z++;
            Debug.Log("To: x: " + X + " z: " + Z);
            return false;
        }

        Debug.Log("To: x: " + X + " z: " + Z);
        return true;
    }

    public bool MoveTR()
    {
        isoTransform.Translate(new Vector3(1, 0, 0));
        X++;

        if (generator.GetMapLayout(X, Z) != SoftwareLevelGenerator.Layout.EMPTY)
        {
            Debug.Log("COLLIDE");
            isoTransform.Translate(new Vector3(-1, 0, 0));
            X--;
            Debug.Log("To: x: " + X + " z: " + Z);
            return false;
        }

        Debug.Log("To: x: " + X + " z: " + Z);
        return true;
    }

    public bool PickUpItem(Direction direction)
    {
        if ((hasElement) || (carrying != null))
        {
            return false;
        }
        else
        {
            var sprite = Resources.Load<Sprite>(HAS_ELEMENT);
            switch (direction)
            {
                case Direction.TopRight:
                    if (generator.GetMapLayout(X + 1, Z) == SoftwareLevelGenerator.Layout.ELEMENT)
                    {
                        carrying = generator.GetObject(X + 1, Z);
                        generator.SetMapLayout(X + 1, Z, SoftwareLevelGenerator.Layout.EMPTY, null);
                        hasElement = true;
                        this.GetComponent<SpriteRenderer>().sprite = sprite;
                        return true;
                    }
                    break;
                case Direction.BottomRight:
                    if (generator.GetMapLayout(X, Z - 1) == SoftwareLevelGenerator.Layout.ELEMENT)
                    {
                        carrying = generator.GetObject(X, Z - 1);
                        generator.SetMapLayout(X, Z - 1, SoftwareLevelGenerator.Layout.EMPTY, null);
                        hasElement = true;
                        this.GetComponent<SpriteRenderer>().sprite = sprite;
                        return true;
                    }
                    break;
                case Direction.BottomLeft:
                    if (generator.GetMapLayout(X - 1, Z) == SoftwareLevelGenerator.Layout.ELEMENT)
                    {
                        carrying = generator.GetObject(X - 1, Z);
                        generator.SetMapLayout(X - 1, Z, SoftwareLevelGenerator.Layout.EMPTY, null);
                        hasElement = true;
                        this.GetComponent<SpriteRenderer>().sprite = sprite;
                        return true;
                    }
                    break;
                case Direction.TopLeft:
                    if (generator.GetMapLayout(X, Z + 1) == SoftwareLevelGenerator.Layout.ELEMENT)
                    {
                        carrying = generator.GetObject(X, Z + 1);
                        generator.SetMapLayout(X, Z + 1, SoftwareLevelGenerator.Layout.EMPTY, null);
                        hasElement = true;
                        this.GetComponent<SpriteRenderer>().sprite = sprite;
                        return true;
                    }
                    break;
            }
        }

        return false;
    }

    public bool DropItem(Direction direction)
    {
        if ((!hasElement) || (carrying == null))
        {
            return false;
        }
        else
        {
            var sprite = Resources.Load<Sprite>(NO_ELEMENT);
            switch (direction)
            {
                case Direction.TopRight:
                    if (generator.GetMapLayout(X + 1, Z) == SoftwareLevelGenerator.Layout.EMPTY)
                    {
                        generator.SetMapLayout(X + 1, Z, SoftwareLevelGenerator.Layout.ELEMENT, carrying);
                        carrying = null;
                        hasElement = false;
                        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
                        renderer.sprite = sprite;
                        return true;
                    }
                    break;
                case Direction.BottomRight:
                    if (generator.GetMapLayout(X, Z - 1) == SoftwareLevelGenerator.Layout.EMPTY)
                    {
                        generator.SetMapLayout(X, Z - 1, SoftwareLevelGenerator.Layout.ELEMENT, carrying);
                        carrying = null;
                        hasElement = false;
                        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
                        renderer.sprite = sprite;
                        return true;
                    }
                    break;
                case Direction.BottomLeft:
                    if (generator.GetMapLayout(X - 1, Z) == SoftwareLevelGenerator.Layout.EMPTY)
                    {
                        generator.SetMapLayout(X - 1, Z, SoftwareLevelGenerator.Layout.ELEMENT, carrying);
                        carrying = null;
                        hasElement = false;
                        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
                        renderer.sprite = sprite;
                        return true;
                    }
                    break;
                case Direction.TopLeft:
                    if (generator.GetMapLayout(X, Z + 1) == SoftwareLevelGenerator.Layout.EMPTY)
                    {
                        generator.SetMapLayout(X, Z + 1, SoftwareLevelGenerator.Layout.ELEMENT, carrying);
                        carrying = null;
                        hasElement = false;
                        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
                        renderer.sprite = sprite;
                        return true;
                    }
                    break;
            }
        }
        return false;
    }


    public void debug()
    {
        generator.PrintMap();
    }
}