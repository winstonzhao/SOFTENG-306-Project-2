using UnityEngine;
using UnityEditor;
using UltimateIsometricToolkit.examples;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		var lg = target as LevelGenerator;
		if(GUILayout.Button("Generate Level")) {
			lg.instantiate();
		}

		if (lg.WorldSize.magnitude > 0 && lg.Map != null && GUILayout.Button("Delete Level"))
		{
			lg.Clear();
		}
	}

}
