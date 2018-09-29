using UltimateIsometricToolkit.physics;
using UnityEditor;

[CustomEditor(typeof(IsoRigidbody))]
public class IsoRigidbodyEditor : Editor {
	IsoRigidbody instance;
	PropertyField[] instance_Fields;
	void OnEnable() {
		instance = target as IsoRigidbody;
		instance_Fields = ExposeProperties.GetProperties(instance);
	}

	public override void OnInspectorGUI() {
		if (instance == null)
			return;
		DrawDefaultInspector();
		ExposeProperties.Expose(instance_Fields);
	}
}

