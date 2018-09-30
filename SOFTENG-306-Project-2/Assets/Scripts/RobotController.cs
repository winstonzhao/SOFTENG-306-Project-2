using Ultimate_Isometric_Toolkit.Scripts.Core;
using UltimateIsometricToolkit.physics;
using UnityEngine;

[RequireComponent(typeof(IsoTransform))]
[RequireComponent(typeof(IsoBoxCollider))]
[RequireComponent(typeof(IsoRigidbody))]
public class RobotController : MonoBehaviour {

    private IsoTransform isoTransform;
    
    private int[,] layoutMap;


    private static int EMPTY = 0;
    private static int ELEMENT = 1;

    private int X = 0;
    private int Z = 0;

    // Use this for initialization
    void Awake()
    {
        isoTransform = this.GetComponent<IsoTransform>();

        layoutMap = new int[9, 7]
        {   {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, ELEMENT},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY},
            {EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY}
        };

}

    // Update is called once per frame
    void Update () {
	}

    public bool Move(int x, int z) {
        for (int i = x; i != 0;) {
            if (i < 0) {
                if (MoveBL()) {
                    i++;
                } else {
                    return false;
                }
            } else {
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

    public bool MoveBL() {
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


    public bool MoveTL() {
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

    public bool MoveBR() {
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

    public bool MoveTR() {
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
}
