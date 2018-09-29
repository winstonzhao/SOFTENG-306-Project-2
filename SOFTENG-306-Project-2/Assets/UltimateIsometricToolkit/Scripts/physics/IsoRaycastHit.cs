using System;
using UltimateIsometricToolkit.physics;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.physics {
	
	/// <summary>
	/// Equivalent of the RaycastHit
	/// </summary>
	public struct IsoRaycastHit {
		//The collider that was hit
		public IsoCollider Collider;
		public float Distance;
		public Vector3 Normal;
		public Vector3 Point;
		public IsoRigidbody IsoRigidbody;
		public IsoTransform IsoTransform;

		public IsoRaycastHit(IsoCollider collider, float distance, Vector3 normal, Vector3 point, IsoRigidbody isoRigidbody, IsoTransform isoTransform) {
			Collider = collider;
			Distance = distance;
			Normal = normal;
			Point = point;
			IsoRigidbody = isoRigidbody;
			IsoTransform = isoTransform;
		}


		public static IsoRaycastHit FromRaycastHit(RaycastHit hit) {
			var ghost = hit.collider.GetComponent<Ghost>();
			if (ghost == null)
				return default(IsoRaycastHit);
		
			//we did not hit a ghost, therefore no IsoCollider
			if (ghost.IsoTransform != null) {
				var isoCollider = ghost.IsoTransform.GetComponent<IsoCollider>();
				var isoRigidbody = ghost.IsoTransform.GetComponent<IsoRigidbody>();
				var isoTransform = ghost.IsoTransform;
				return new IsoRaycastHit(isoCollider, hit.distance, hit.normal, hit.point, isoRigidbody, isoTransform);
			} else {
				Debug.Log(ghost.name);
				ghost.gameObject.hideFlags = HideFlags.None;
				throw new Exception();
			}
		}

	}
}
