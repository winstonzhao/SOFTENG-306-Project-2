using System;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.External;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UltimateIsometricToolkit.physics;
using UnityEngine;

namespace Assets.Scripts {
	[AddComponentMenu("UIT/Physics/IsoMeshCollider")]
	public class IsoMeshCollider : IsoCollider {

		[HideInInspector, SerializeField] private Vector3 _scale = Vector3.one;
		[HideInInspector, SerializeField]private bool _convex;

		[ExposeProperty]
		public bool Convex {
			get { return _convex; }
			set {
				_convex = value;
				if(MCollider != null)
					MCollider.convex = _convex;
			}
		}

		[ExposeProperty]
		public Mesh Mesh {
			get { return MCollider.sharedMesh; }
			set { MCollider.sharedMesh = value; }
		}
		private MeshCollider MCollider {
			get {
				return Collider as MeshCollider;
			}
		}

		[ExposeProperty]
		public Vector3 Scale {
			get { return _scale; }
			set {
				_scale = new Vector3(Mathf.Clamp(value.x, 0, Mathf.Infinity), Mathf.Clamp(value.y, 0, Mathf.Infinity), Mathf.Clamp(value.z, 0, Mathf.Infinity)); ;
				if (MCollider != null)
					MCollider.transform.localScale = _scale;
			}
		}

		[Obsolete("offset no longer supported for performance reasons")]
	//	[ExposeProperty]
		public Vector3 Offset { get; set; }
		protected override Collider instantiateCollider(GameObject obj) {
			var collider = obj.AddComponent<MeshCollider>();
			collider.convex = Convex;
			collider.sharedMesh = Mesh;
			obj.transform.localScale = Scale;
			return collider;

		}

		public override void Draw() {
			if (MCollider == null || MCollider.sharedMesh == null) {
				Debug.Log(name + ", IsoMeshCollider is missing a Mesh to show bounds. \n Add a mesh or delete the IsoMeshCollider component");
				return;
			}
			if (IsoTransform == null)
				IsoTransform = GetComponent<IsoTransform>();
			Gizmos.color = Color.green;
			GizmosExtension.DrawIsoMesh(Mesh,IsoTransform.Position,Scale);
		}
	}
}
