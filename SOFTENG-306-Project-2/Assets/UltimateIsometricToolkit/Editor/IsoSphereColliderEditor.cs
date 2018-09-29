using UnityEngine;
using UltimateIsometricToolkit.physics;
using UnityEditor;

[CustomEditor(typeof(IsoSphereCollider)), DisallowMultipleComponent]
public class IsoSphereColliderEditor : Editor {

	IsoSphereCollider instance;
	PropertyField[] instance_Fields;
	void OnEnable() {
		instance = target as IsoSphereCollider;
		instance_Fields = ExposeProperties.GetProperties(instance);
	}

	public override void OnInspectorGUI() {
		if (instance == null)
			return;
		DrawDefaultInspector();
		ExposeProperties.Expose(instance_Fields);
	}
}
