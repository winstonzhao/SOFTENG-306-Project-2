using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.External;
using Ultimate_Isometric_Toolkit.Scripts.Utils;

namespace UltimateIsometricToolkit.physics
{
    [AddComponentMenu("UIT/Physics/IsoSphereCollider")]
    public class IsoSphereCollider : IsoCollider
    {
        [HideInInspector, SerializeField]
        private Vector3 _center;

        [HideInInspector, SerializeField]
        private float _radius;

        [ExposeProperty]
        public Vector3 Center
        {
            get { return _center; }
            set
            {
                _center = value;
                if (sphereCollider != null)
                    sphereCollider.center = value;
            }
        }

        [ExposeProperty]
        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = Mathf.Clamp(value, 0, Mathf.Infinity);
                if (sphereCollider != null)
                    sphereCollider.radius = _radius;
            }
        }

        private SphereCollider sphereCollider
        {
            get { return Collider as SphereCollider; }
        }

        protected override Collider instantiateCollider(GameObject obj)
        {
            var collider = obj.AddComponent<SphereCollider>();
            collider.radius = IsoTransform.Size.magnitude;
            collider.center = Vector3.zero;
            return collider;
        }

        public override void Draw()
        {
            Gizmos.color = Color.green;
            GizmosExtension.DrawIsoWireSphere(IsoTransform.Position + Center, Radius, 10.0f);
        }
    }
}
