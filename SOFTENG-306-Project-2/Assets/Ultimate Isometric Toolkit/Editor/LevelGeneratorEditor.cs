using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		if(GUILayout.Button("Init map")) {
			((LevelGenerator)target).instantiate();
		}
	}

}
