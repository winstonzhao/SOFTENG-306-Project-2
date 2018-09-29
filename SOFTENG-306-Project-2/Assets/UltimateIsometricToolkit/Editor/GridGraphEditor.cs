using Ultimate_Isometric_Toolkit.Scripts.Pathfinding;
using UnityEditor;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Editor {
	[CustomEditor(typeof(GridGraph))]
	public class GridGraphEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			GridGraph myScript = (GridGraph)target;
			if (GUILayout.Button("Update Graph")) {
				myScript.UpdateGraph();
				EditorUtility.SetDirty(myScript);
			}
		}
	}
}
