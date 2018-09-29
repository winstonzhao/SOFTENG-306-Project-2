using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

[ExecuteInEditMode]
public class RobotController : MonoBehaviour {

    public int Speed = 5;

    private IsoTransform isoTransform;

    private Ghost ghost;

    // Use this for initialization
    void Start()
    {
        isoTransform = this.GetComponent<IsoTransform>();

        // Setting up ghost object for collision detection
        var gameObj = new GameObject
        {
            name = "Ghost_" + transform.name
        };
        ghost = gameObj.AddComponent<Ghost>();
        var ghostTransform = ghost.GetOrAddComponent<IsoTransform>();
        var collider = gameObj.AddComponent<BoxCollider>();
        gameObj.AddComponent<IsoCollider>();
        collider.size = new Vector3(isoTransform.Size.x, isoTransform.Size.y, isoTransform.Size.z);

        ghostTransform.Position = new Vector3(isoTransform.Position.x, isoTransform.Position.y, isoTransform.Position.z);
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
        ghost.transform.Translate(new Vector3(-1, 0, 0));

        if (ghost.GetComponent<BoxCollider>().isTrigger) {
            Debug.Log("ghost collide");
            ghost.transform.Translate(new Vector3(1, 0, 0));
            return false;
        } 
        else 
        {
            isoTransform.Translate(new Vector3(-1, 0, 0));
            return true;
        }
    }


    public bool MoveTL() {
        ghost.transform.Translate(new Vector3(0, 0, 1));

        if (ghost.GetComponent<BoxCollider>().isTrigger)
        {
            Debug.Log("ghost collide");
            ghost.transform.Translate(new Vector3(0, 0, -1));
            return false;
        }
        else
        {
            isoTransform.Translate(new Vector3(0, 0, 1));
            return true;
        }
    }

    public bool MoveBR() {
        ghost.transform.Translate(new Vector3(0, 0, -1));

        if (ghost.GetComponent<BoxCollider>().isTrigger)
        {
            ghost.transform.Translate(new Vector3(0, 0, 1));
            Debug.Log("ghost collide");
            return false;
        }
        else
        {
            isoTransform.Translate(new Vector3(0, 0, -1));
            return true;
        }
    }

    public bool MoveTR() {
        ghost.transform.Translate(new Vector3(1, 0, 0));

        if (ghost.GetComponent<BoxCollider>().isTrigger)
        {
            Debug.Log("ghost collide");
            ghost.transform.Translate(new Vector3(-1, 0, 0));
            return false;
        }
        else
        {
            isoTransform.Translate(new Vector3(1, 0, 0));
            return true;
        }
    }
}
