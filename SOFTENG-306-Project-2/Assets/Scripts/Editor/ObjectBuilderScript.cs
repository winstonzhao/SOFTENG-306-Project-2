using UnityEngine;
using UnityEditor;

/// <summary>
/// GUI button for base floor generator - clicking the button rebuilds the floor tiles
/// </summary>
[CustomEditor(typeof(BaseFloorGenerator))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BaseFloorGenerator myScript = (BaseFloorGenerator) target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.RePaint();
        }
    }
}
