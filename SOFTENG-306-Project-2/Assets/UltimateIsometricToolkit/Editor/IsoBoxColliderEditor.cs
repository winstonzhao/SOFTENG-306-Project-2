using UnityEngine;
using UltimateIsometricToolkit.physics;
using UnityEditor;

[CustomEditor(typeof(IsoBoxCollider)),DisallowMultipleComponent]
public class IsoBoxColliderEditor : Editor {

	IsoBoxCollider instance;
	PropertyField[] instance_Fields;
	void OnEnable() {
		instance = target as IsoBoxCollider;
		instance_Fields = ExposeProperties.GetProperties(instance);
	}

	public override void OnInspectorGUI() {
		if (instance == null)
			return;
		DrawDefaultInspector();
		ExposeProperties.Expose(instance_Fields);
	}
}
