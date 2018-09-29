﻿using UnityEditor;

[CustomEditor(typeof(RobotController))]
public class TestRobotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RobotController myScript = (RobotController)target;
        if (UnityEngine.GUILayout.Button("Move TL"))
        {
            myScript.MoveTL();
        }

        if (UnityEngine.GUILayout.Button("Move BL"))
        {
            myScript.MoveBL();
        }

        if (UnityEngine.GUILayout.Button("Move TR"))
        {
            myScript.MoveTR();
        }

        if (UnityEngine.GUILayout.Button("Move BR"))
        {
            myScript.MoveBR();
        }
    }
}
