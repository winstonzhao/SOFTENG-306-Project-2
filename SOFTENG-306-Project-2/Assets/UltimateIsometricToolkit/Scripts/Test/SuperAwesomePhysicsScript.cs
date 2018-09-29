using UltimateIsometricToolkit.physics;
using UnityEngine;

namespace UltimateIsometricToolkit.examples {
	/// <summary>
	/// Tests if OnCollision and OnTrigger callbacks get invoked properly
	/// </summary>
	public class SuperAwesomePhysicsScript : MonoBehaviour {

		void OnIsoCollisionEnter(IsoCollision isoCollisionInfo) {
			Debug.Log(gameObject.name + " enters collision with " + isoCollisionInfo.isoCollider.name + ", awesome!");
		}

		void OnIsoCollisionStay(IsoCollision isoCollisionInfo) {
			Debug.Log(gameObject.name + " stays on collision with " + isoCollisionInfo.isoCollider.name + ", awesome!");
		}

		void OnIsoCollisionExit(IsoCollision isoCollisionInfo) {
			Debug.Log(gameObject.name + " exits collision with " + isoCollisionInfo.isoCollider.name + ", awesome!");
		}

		void OnIsoTriggerEnter(IsoCollider isoCollider) {
			Debug.Log(gameObject.name + " enters trigger with " + isoCollider.gameObject.name + ", awesome!");
		}

		void OnIsoTriggerStay(IsoCollider isoCollider) {
			Debug.Log(gameObject.name + " stays on trigger with " + isoCollider.gameObject.name + ", awesome!");
		}

		void OnIsoTriggerExit(IsoCollider isoCollider) {
			Debug.Log(gameObject.name + " exits trigger with " + isoCollider.gameObject.name + ", awesome!");
		}

	}
}
