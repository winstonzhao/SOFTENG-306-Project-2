using UnityEngine;
using UltimateIsometricToolkit.physics;
using UnityEditor;

[CustomEditor(typeof(IsoCapsuleCollider)), DisallowMultipleComponent]
public class IsoCapsuleColliderEditor : Editor {

	IsoCapsuleCollider instance;
	PropertyField[] instance_Fields;
	void OnEnable() {
		instance = target as IsoCapsuleCollider;
		instance_Fields = ExposeProperties.GetProperties(instance);
	}

	public override void OnInspectorGUI() {
		if (instance == null)
			return;
		DrawDefaultInspector();
		ExposeProperties.Expose(instance_Fields);
	}
}
