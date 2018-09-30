using System.Collections;
using System.Collections.Generic;
using UltimateIsometricToolkit.physics;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.Pathfinding;
using UnityEngine;

public class CivilVehicleController : MonoBehaviour {

    public static CivilVehicleController instance;
    public List<AstarAgent> AstarAgents = new List<AstarAgent>();
    public List<GoalAgent> Goals = new List<GoalAgent>();

    private void Awake()

    {
        Debug.Log("CivilVehicleController Alive");

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

    // Update is called once per frame
    //public void Update()
    //{

    //    //raycast when mouse clicked
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        run();

    //        //         var isoRay = Isometric.MouseToIsoRay();
    //        //IsoRaycastHit isoRaycastHit;
    //        //if (IsoPhysics.Raycast(isoRay, out isoRaycastHit)) {
    //        //	Debug.Log("Moving to " + isoRaycastHit.Point);
    //        //             AstarAgent.MoveTo(isoRaycastHit.Point);
    //        //         }
    //    }
    //}

    public void run()
    {
        foreach (GoalAgent goal in Goals)
        {
            foreach (AstarAgent agent in AstarAgents)
            {
                if (agent.Type == goal.GoalType)
                {
                    agent.MoveTo(goal.GetComponentInParent<IsoTransform>().Position);
                }
            }
        }
    }

}
