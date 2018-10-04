using Ultimate_Isometric_Toolkit.Scripts.External;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

namespace UltimateIsometricToolkit.physics
{
    [AddComponentMenu("UIT/Physics/IsoBoxCollider")]
    public class IsoBoxCollider : IsoCollider
    {
        [HideInInspector, SerializeField]
        private Vector3 _size = Vector3.zero;

        [HideInInspector, SerializeField]
        private Vector3 _center = Vector3.zero;

        [ExposeProperty]
        public Vector3 Size
        {
            get { return _size; }
            set
            {
                _size = new Vector3(Mathf.Clamp(value.x, 0, Mathf.Infinity), Mathf.Clamp(value.y, 0, Mathf.Infinity),
                    Mathf.Clamp(value.z, 0, Mathf.Infinity));

                if (BoxCollider != null)
                    BoxCollider.size = _size;
            }
        }

        [ExposeProperty]
        public Vector3 Center
        {
            get { return _center; }
            set
            {
                _center = value;
                if (BoxCollider != null)
                    BoxCollider.center = value;
            }
        }

        private BoxCollider BoxCollider
        {
            get { return Collider as BoxCollider; }
        }

        protected override Collider instantiateCollider(GameObject obj)
        {
            var collider = obj.AddComponent<BoxCollider>();
            Size = Size == Vector3.zero ? IsoTransform.Size : Size;
            Center = _center;
            collider.size = Size;
            collider.center = Center;
            return collider;
        }

        public override void Draw()
        {
            base.Draw();
            Gizmos.color = Color.green;
            Size = Size == Vector3.zero ? IsoTransform.Size : Size;
            if (IsoTransform != null)
                GizmosExtension.DrawIsoWireCube(IsoTransform.Position + Center, Size);
        }
    }
}
