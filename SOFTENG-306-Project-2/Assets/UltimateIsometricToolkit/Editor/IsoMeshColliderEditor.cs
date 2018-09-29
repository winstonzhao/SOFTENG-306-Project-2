using Assets.Scripts;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IsoMeshCollider)), DisallowMultipleComponent]
public class IsoMeshColliderEditor : Editor {

	IsoMeshCollider instance;
	PropertyField[] instance_Fields;
	void OnEnable() {
		instance = target as IsoMeshCollider;
		instance_Fields = ExposeProperties.GetProperties(instance);
	}

	public override void OnInspectorGUI() {
		if (instance == null)
			return;
		DrawDefaultInspector();
		ExposeProperties.Expose(instance_Fields);
	}
}
