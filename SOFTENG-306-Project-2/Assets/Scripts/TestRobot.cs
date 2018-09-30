using UnityEditor;

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

        if (UnityEngine.GUILayout.Button("PickUp TL")) 
        {
            myScript.PickUpItem(RobotController.Direction.TopLeft);
        }

        if (UnityEngine.GUILayout.Button("PickUp BL"))
        {
            myScript.PickUpItem(RobotController.Direction.BottomLeft);
        }

        if (UnityEngine.GUILayout.Button("PickUp TR"))
        {
            myScript.PickUpItem(RobotController.Direction.TopRight);
        }

        if (UnityEngine.GUILayout.Button("PickUp BR"))
        {
            myScript.PickUpItem(RobotController.Direction.BottomRight);
        }

        if (UnityEngine.GUILayout.Button("Drop TL"))
        {
            myScript.DropItem(RobotController.Direction.TopLeft);
        }

        if (UnityEngine.GUILayout.Button("Drop BL"))
        {
            myScript.DropItem(RobotController.Direction.BottomLeft);
        }

        if (UnityEngine.GUILayout.Button("Drop TR"))
        {
            myScript.DropItem(RobotController.Direction.TopRight);
        }

        if (UnityEngine.GUILayout.Button("Drop BR"))
        {
            myScript.DropItem(RobotController.Direction.BottomRight);
        }

        if (UnityEngine.GUILayout.Button("Debug")) {
            myScript.debug();
        }

    }
}
