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

    private int[,] layoutMap;
    private GameObject[,] objectMap;

    private static int EMPTY = 0;
    private static int ELEMENT = 1;
    private static int PADDING = -1;

    private int X = 1;
    private int Z = 1;
    private int numElements = 6;
    private bool hasElement = false;

    private static string NO_ELEMENT = "Assets/Sprites/software_minigame/alienBeige 1.png";
    private static string HAS_ELEMENT = "Assets/Sprites/software_minigame/alienBeige 2.png";
    private static string ELEMENT_PREFAB = "Assets/Prefabs/software_minigame/test_item.prefab";

    // Use this for initialization
    void Awake()
    {
        isoTransform = this.GetComponent<IsoTransform>();

        layoutMap = new int[11, 9]
        {   {PADDING, PADDING, PADDING, PADDING, PADDING, PADDING, PADDING, PADDING, PADDING},
            {PADDING, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, PADDING},
            {PADDING, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, PADDING},
            {PADDING, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, PADDING},
            {PADDING, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, PADDING},
            {PADDING, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, PADDING},
            {PADDING, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, PADDING},
            {PADDING, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, PADDING},
            {PADDING, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, PADDING},
            {PADDING, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, PADDING},
            {PADDING, PADDING, PADDING, PADDING, PADDING, PADDING, PADDING, PADDING, PADDING},
        };
        objectMap = new GameObject[11, 9];

        Object prefab = AssetDatabase.LoadAssetAtPath(ELEMENT_PREFAB, typeof(GameObject));
        GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        obj.GetComponent<IsoTransform>().Position = new Vector3(0, 0.8f, 6);
        obj.AddComponent<ArrayElement>();
        layoutMap[1, 7] = ELEMENT;
        objectMap[1, 7] = obj;
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

        if (layoutMap[X, Z] != EMPTY)
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
        if (layoutMap[X, Z] != EMPTY)
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

        if (layoutMap[X, Z] != EMPTY)
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

        if (layoutMap[X, Z] != EMPTY)
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

    public bool PickUp(Direction direction)
    {
        if (hasElement) {
            return false;
        }
        else
        {
            Sprite sprite = (UnityEngine.Sprite)AssetDatabase.LoadAssetAtPath(HAS_ELEMENT, typeof(Sprite));
            switch (direction)
            {
                case Direction.TopRight:
                    if (layoutMap[X + 1, Z] == ELEMENT) {
                        DestroyImmediate(objectMap[X, Z + 1]);
                        layoutMap[X + 1, Z] = EMPTY;
                        hasElement = true;
                        this.GetComponent<SpriteRenderer>().sprite = sprite;
                        return true;
                    }
                    break;
                case Direction.BottomRight:
                    if (layoutMap[X, Z - 1] == ELEMENT)
                    {
                        DestroyImmediate(objectMap[X, Z - 1]);
                        layoutMap[X, Z - 1] = EMPTY;
                        hasElement = true;
                        this.GetComponent<SpriteRenderer>().sprite = sprite;
                        return true;
                    }
                    break;
                case Direction.BottomLeft:
                    if (layoutMap[X - 1, Z] == ELEMENT)
                    {
                        DestroyImmediate(objectMap[X - 1, Z]);
                        layoutMap[X - 1, Z] = EMPTY;
                        hasElement = true;
                        this.GetComponent<SpriteRenderer>().sprite = sprite;
                        return true;
                    }
                    break;
                case Direction.TopLeft:
                    if (layoutMap[X, Z + 1] == ELEMENT)
                    {
                        DestroyImmediate(objectMap[X, Z + 1]);
                        layoutMap[X, Z + 1] = EMPTY;
                        hasElement = true;
                        this.GetComponent<SpriteRenderer>().sprite = sprite;
                        return true;
                    }
                    break;
            }
        }

        return false;
    }

    public enum Direction
    {
        TopRight = 0,
        BottomRight = 1,
        BottomLeft = 2,
        TopLeft = 3
    }
}