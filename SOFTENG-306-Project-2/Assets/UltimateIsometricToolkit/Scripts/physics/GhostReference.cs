using Ultimate_Isometric_Toolkit.Scripts.Core;
using UltimateIsometricToolkit.physics;
using UnityEngine;

/// <summary>
/// Class that holds a reference to the GhostObject for IsoColliders and IsoRigidbody
/// </summary>
[DisallowMultipleComponent]
public class    GhostReference : MonoBehaviour {

	public GameObject GhostObject { get; private set; }

	#region  Unity Events

	void Awake() {
		hideFlags = HideFlags.HideInInspector;
		var isoTransform = GetComponent<IsoTransform>();
		if (isoTransform) {
			var ghostObj = new GameObject(gameObject.name + "_ghost") { hideFlags =  HideFlags.HideInHierarchy | HideFlags.NotEditable };
			ghostObj.AddComponent<Ghost>().Setup(isoTransform); ;
			DontDestroyOnLoad(ghostObj);
			GhostObject = ghostObj;
		}
	}

	void OnDestroy() {
		if (GhostObject != null) {
			DestroyImmediate(GhostObject);
			GhostObject = null;
		}
	}

	/// <summary>
	/// Removes collider from ghost if attached, and deletes entire ghost if no rigidbody attached
	/// </summary>
	public void RemoveCollider() {
		var ghostcollider = GhostObject.GetComponent<Collider>();
		Destroy(ghostcollider);
		var ghostRigidbody = GhostObject.GetComponent<Rigidbody>();
		if(ghostRigidbody == null)
			Destroy(this);

	}
	/// <summary>
	/// Removes rigidbody from ghost if attached, and deletes entire ghost if no collider attached
	/// </summary>
	public void RemoveRigidBody() {
		var ghostRigidbody = GhostObject.GetComponent<Rigidbody>();
		Destroy(ghostRigidbody);
		var ghostCollider = GhostObject.GetComponent<Collider>();
		if (ghostCollider == null)
			Destroy(this);
	}
	#endregion
}
