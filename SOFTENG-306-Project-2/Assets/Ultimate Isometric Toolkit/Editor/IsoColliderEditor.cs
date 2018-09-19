using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IsoCollider), true), CanEditMultipleObjects]
class IsoColliderEditor : Editor {
    void OnSceneGUI() {
        var t = ((IsoCollider)target);
        if(t.GetComponent<IsoObject>() != null)
            DrawWireCube(t.GetComponent<IsoObject>().Position, t.GetComponent<IsoObject>().Size);
    }


   /// <summary>
   /// Draw wire cube for collider on isoprojection
   /// </summary>
   /// <param name="position"></param>
   /// <param name="size"></param>
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

