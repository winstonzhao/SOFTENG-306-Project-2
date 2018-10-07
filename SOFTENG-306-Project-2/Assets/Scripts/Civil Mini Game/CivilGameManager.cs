using System.Collections;
using System.Collections.Generic;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

public class CivilGameManager : MonoBehaviour {

    public static CivilGameManager instance;
    public string playerName  = "Anonymous";


    private static float lastClickTime = 0;
    private static float catchTime = 0.2f;


    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPlayerName(string playerName)
    {
        instance.playerName = playerName;
    }

    public static void CheckMouseClickForRotation() // Super
    {
        //mouse ray in isometric coordinate system 
        var isoRay = Isometric.MouseToIsoRay();

        //do an isometric raycast on double left mouse click 
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < catchTime)
            {
                //double click
                Debug.Log("double click");
                IsoRaycastHit isoRaycastHit;
                if (IsoPhysics.Raycast(isoRay, out isoRaycastHit))
                {
                    GameObject hitObject = isoRaycastHit.Collider.gameObject;
                    Debug.Log("we clicked on " + hitObject.name + " at " + isoRaycastHit.Point + " tagged as " + hitObject.tag);
                    if (hitObject.tag == "BuildingBlock")
                    {
                        hitObject.GetComponent<DraggableIsoItem>().Rotate();
                        Debug.Log(hitObject.name + " rotated to direction " + hitObject.GetComponent<DraggableIsoItem>().direction);
                    }
                }
            }
            lastClickTime = Time.time;
        }
    }

    public static void ToggleDialogDisplay(Canvas canvas, string groupName, bool show)    // super
    {
        GameObject group = FindObject(canvas.gameObject, groupName).gameObject;
        if (group != null)
        {
            group.SetActive(show);
        }
    }

    /**
 * Find an object in a parent object including parent objects
 */
    public static GameObject FindObject(GameObject parent, string name)    // super
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
