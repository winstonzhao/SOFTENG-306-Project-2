using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.External;
using Ultimate_Isometric_Toolkit.Scripts.Utils;


namespace UltimateIsometricToolkit.physics
{
	[ExecuteInEditMode,AddComponentMenu("UIT/Physics/IsoCapsuleCollider")]

	public class IsoCapsuleCollider : IsoCollider {
#if UNITY_EDITOR
		private Mesh capsuleMesh;
#endif
		[HideInInspector, SerializeField]private Vector3 _center;
		[HideInInspector, SerializeField]private float _height = 1;
		[HideInInspector, SerializeField]private float _radius = 0.25f;
		private CapsuleCollider capsuleCollider {
			get {
				return Collider as CapsuleCollider;
			}
		}

		[ExposeProperty]
		public Vector3 Center {
			get {
				return _center;
				
			}
			set {
				_center = value;
				if (capsuleCollider != null)
					capsuleCollider.center = value;
			}
		}

		[ExposeProperty]
		public float Radius {
			get {
				return _radius;
			}
			set {
				_radius = Mathf.Clamp(value, 0, Mathf.Infinity);
				if (capsuleCollider != null)
					capsuleCollider.radius = _radius;
				#if UNITY_EDITOR
				capsuleMesh = ProcedualMeshes.GenerateCapsule(Height, Radius);
				#endif
			}
		}

		[ExposeProperty]
		public float Height {
			get {
				return _height;
			}
			set {
				_height = Mathf.Clamp(value,0,Mathf.Infinity);
				if (capsuleCollider != null)
					capsuleCollider.height = _height;
#if UNITY_EDITOR
				capsuleMesh = ProcedualMeshes.GenerateCapsule(Height, Radius);
				#endif
			}
		}

		void OnEnable() {
			#if UNITY_EDITOR
			capsuleMesh = ProcedualMeshes.GenerateCapsule(Height, Radius);
			#endif
		}

		void OnDisable() {
			#if UNITY_EDITOR
			capsuleMesh = null;
			#endif
		}
		

		protected override Collider instantiateCollider(GameObject obj)
		{
			var collider = obj.AddComponent<CapsuleCollider>();
			collider.radius = Mathf.Max(IsoTransform.Size.x, IsoTransform.Size.y)/ 4f;
			collider.height = IsoTransform.Size.y;
			collider.center = Vector3.zero;
			return collider;
		}

		public override void Draw()
		{
			Gizmos.color = Color.green;
			#if UNITY_EDITOR
			GizmosExtension.DrawIsoMesh(capsuleMesh, IsoTransform.Position + Center, Vector3.one);
			#endif
			
		}
	}
}
