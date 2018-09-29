using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using UltimateIsometricToolkit.physics;

/// <summary>
/// Use this as an example for a controller with collisiondetection
/// </summary>

namespace UltimateIsometricToolkit.controller {
	[ExecuteInEditMode, DisallowMultipleComponent, AddComponentMenu("UIT/CharacterController/AdvancedController")]
	public class AdvancedIsoObjectController : MonoBehaviour {

		public float Speed = 10;
		public float JumpForce = 200;

		private IsoTransform _isoTransform;
		private IsoRigidbody _isoRigidbody;

		void Awake() {
			_isoTransform = this.GetOrAddComponent<IsoTransform>(); //better than requirecomponent
			this.GetOrAddComponent<IsoCollider>();
			_isoRigidbody = this.GetOrAddComponent<IsoRigidbody>();

		}

		void Update() {
			//translate on isotransform
			_isoRigidbody.Velocity = new Vector3(Input.GetAxis("Vertical") * Speed, _isoRigidbody.Velocity.y, -Input.GetAxis("Horizontal") * Speed);
			
			if (Input.GetKeyDown("space")) {
				//check vertical distance 
				IsoRaycastHit hit;
				var maxDistanceToGround = 1f;
				if (IsoPhysics.Raycast(_isoTransform.Position, Vector3.down, out hit, maxDistanceToGround)) {
					//add upp force 
					_isoRigidbody.AddForce(Vector3.up*JumpForce);
				}
			}
		}
	}
}
