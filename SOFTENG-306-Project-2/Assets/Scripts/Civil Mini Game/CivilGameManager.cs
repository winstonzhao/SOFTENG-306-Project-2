using System.Collections;
using System.Collections.Generic;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

/**
 * This class is a singleton class for managing the civil mini game across multiple levels.
 * This class also includes a few util methods that can be used independently by any level.
 */
public class CivilGameManager : MonoBehaviour {

    // singleton CivilGameManager instance
    public static CivilGameManager instance;
    
    // player name, default is "Anonymous"
    public string playerName  = "Anonymous";


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

    public void SetPlayerName(string playerName)
    {
        instance.playerName = playerName;
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
