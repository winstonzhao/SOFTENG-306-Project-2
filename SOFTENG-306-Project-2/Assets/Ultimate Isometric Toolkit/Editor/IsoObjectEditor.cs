using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

//custom editor for all IsoObjects and its subclasses
[CustomEditor(typeof(IsoObject), true), CanEditMultipleObjects]
class IsoObjectEditor : Editor {

    public static Vector3 center;
    public static Vector3 delta;

    public static bool centerNeedsUpdate = false;


    public static IsoObject[] lastTargets;


    void OnSceneGUI () {
        //calc the custom handle position in sceneview
        var isoHandlePos = Isometric.isoProjection(center);
        var t = (IsoObject)target;
      

        if (GUI.changed) {
            centerNeedsUpdate = true;
        }
        var x = 0f;
        var y = 0f;
        var z = 0f;
        //Get deltas for (north, up, west)
        try {
            x = handleX(isoHandlePos);
            y = handleY(isoHandlePos);
            z = handleZ(isoHandlePos);
        } catch (ArithmeticException) {

        }


        //snaps the target to closest multiple
        if (IsoSnapping.doSnap) {
            Vector3 vec = IsoSnapping.Ceil(new Vector3(x, y, z));
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        if (Event.current.shift) {

            x *= 0.1f;
            y *= 0.1f;
            z *= 0.1f;
        }

		if(t.displayBounds) {
			DrawWireCube(t.Position, t.Size);
		}

        switch (GUI.GetNameOfFocusedControl()) {
            case "x":
                delta = new Vector3(x, 0, 0);
                break;
            case "y":
                delta = new Vector3(0, y, 0);
                break;
            case "z":
                delta = new Vector3(0, 0, z);
                break;
            default:
                break;
        }
        t.Position += delta;
       
            EditorUtility.SetDirty(t);
        
    }

	public override void OnInspectorGUI() {
		var isoObject = (IsoObject)target;
		DrawDefaultInspector();
		//call property for side effects in "set"
		isoObject.Size = isoObject.Size;
		isoObject.Position = isoObject.Position;

		serializedObject.ApplyModifiedProperties();
		serializedObject.Update();

	}

    //enable - disable last tool
    void OnEnable() {
        Tools.current = Tool.None;
        EditorApplication.update += new EditorApplication.CallbackFunction(updateCenter);
    }

    void OnDisable() {

    }


    IsoObject[] getFromTargets() {
        List<IsoObject> objs = new List<IsoObject>();
        foreach (object t in targets) {
            var isoobj = (IsoObject)t;

            if (isoobj != null) {
                objs.Add(isoobj);
            }
        }
        return objs.ToArray();
    }

    public void updateCenter() {
        var currentTargets = getFromTargets();
        if (lastTargets != currentTargets || lastTargets == null) {
            centerNeedsUpdate = true;
            lastTargets = currentTargets;
        }
        if (centerNeedsUpdate) {
            IsoObject[] isoObjs = currentTargets;
            center = Vector3.zero;
            foreach (IsoObject obj in isoObjs) {
                center += obj.Position;
            }
            center = center / (float)isoObjs.Length;
            centerNeedsUpdate = false;
        }
    }


    /// <summary>
    /// creates a handle in Up axis. return the delta movement on the isometric up axis.
    /// </summary>
    /// <param name="isoPosition"></param> the isometric position of our object
    /// <returns></returns>
    float handleZ(Vector3 isoPosition) {
        GUI.SetNextControlName("z");
        Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.75f);
        var dir = Isometric.isoProjection(Isometric.vectorToIsoDirection(IsoDirection.Up));
        var delta = Handles.Slider(isoPosition, dir);
        var d = delta - isoPosition;

        var res = d.magnitude * ((Math.Sign(d.y) == Math.Sign(dir.y)) ? 1 : -1);
        return res;
    }


    /// <summary>
    /// creates a handle in forward axis. return the delta movement on the isometric north axis.
    /// </summary>
    /// <param name="isoPosition"></param> the isometric position of our object. Pass the transform.position component
    /// <returns></returns>
    float handleX(Vector3 pos) {
        GUI.SetNextControlName("x");
        Handles.color = new Color(0.0f, 0.0f, 1.0f, 0.75f);
        var dir = Isometric.isoProjection(Isometric.vectorToIsoDirection(IsoDirection.North));
        var delta = Handles.Slider(pos,dir);
        var d = delta - pos;
        
        var res = d.magnitude * ((Math.Sign(d.x) == Math.Sign(dir.x)) ? 1 : -1);
        return res;
    }

    /// <summary>
    /// creates a handle in left axis. return the delta movement on the isometric wEst axis.
    /// </summary>
    /// <param name="isoPosition"></param> the isometric position of our object. Pass the transform.position component
    /// <returns></returns>
    float handleY(Vector3 pos) {
        GUI.SetNextControlName("y");
        Handles.color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
        var dir =  Isometric.isoProjection(Isometric.vectorToIsoDirection(IsoDirection.West));
        var delta = Handles.Slider(pos, dir);
        var d = delta - pos;

        var res = d.magnitude * ((Math.Sign(d.x) == Math.Sign(dir.x)) ? 1 : -1);
        return res;
    }


	public static void DrawWireCube(Vector3 position, Vector3 size)
	{
		var half = size / 2;
		// draw front
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(-half.x, -half.y, half.z)), Isometric.isoProjection(position + new Vector3(half.x, -half.y, half.z)));
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(-half.x, -half.y, half.z)), Isometric.isoProjection(position + new Vector3(-half.x, half.y, half.z)));
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(half.x, half.y, half.z)), Isometric.isoProjection(position + new Vector3(half.x, -half.y, half.z)));
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(half.x, half.y, half.z)), Isometric.isoProjection(position + new Vector3(-half.x, half.y, half.z)));
		// draw back
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(-half.x, -half.y, -half.z)), Isometric.isoProjection(position + new Vector3(half.x, -half.y, -half.z)));
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(-half.x, -half.y, -half.z)), Isometric.isoProjection(position + new Vector3(-half.x, half.y, -half.z)));
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(half.x, half.y, -half.z)), Isometric.isoProjection(position + new Vector3(half.x, -half.y, -half.z)));
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(half.x, half.y, -half.z)), Isometric.isoProjection(position + new Vector3(-half.x, half.y, -half.z)));
		// draw corners
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(-half.x, -half.y, -half.z)), Isometric.isoProjection(position + new Vector3(-half.x, -half.y, half.z)));
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(half.x, -half.y, -half.z)), Isometric.isoProjection(position + new Vector3(half.x, -half.y, half.z)));
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(-half.x, half.y, -half.z)), Isometric.isoProjection(position + new Vector3(-half.x, half.y, half.z)));
		Handles.DrawLine(Isometric.isoProjection(position + new Vector3(half.x, half.y, -half.z)), Isometric.isoProjection(position + new Vector3(half.x, half.y, half.z)));
	}

    
}