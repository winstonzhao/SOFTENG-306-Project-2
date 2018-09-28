using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BaseFloorGenerator))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BaseFloorGenerator myScript = (BaseFloorGenerator)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.RePaint();
        }
    }
}
