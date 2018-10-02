using System.Collections;
using System.Linq;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.External;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using UnityEngine;

namespace UltimateIsometricToolkit.physics
{
    /// <summary>
    /// Wrapper class for Rigidbody
    /// </summary>
    [RequireComponent(typeof(IsoTransform)), DisallowMultipleComponent, AddComponentMenu("UIT/Physics/IsoRigidbody")]
    public class IsoRigidbody : MonoBehaviour
    {
        [HideInInspector, SerializeField]
        private float _mass = 1;

        [HideInInspector, SerializeField]
        private float _drag;

        [HideInInspector, SerializeField]
        private bool _useGravity = true;

        [HideInInspector, SerializeField]
        private bool _isKinematric;

        [HideInInspector, SerializeField]
        private CollisionDetectionMode _collisionDetectionMode;

        [HideInInspector, SerializeField]
        private Vector3 _centerOfMass;

        [HideInInspector, SerializeField]
        private RigidbodyInterpolation _rigidbodyInterpolation;

        [HideInInspector, SerializeField]
        private float _maxDepenetrationVelocity;

        [HideInInspector, SerializeField]
        private float _sleepThreshold;

        Rigidbody _rigidbody;

        private Rigidbody Rigidbody
        {
            get { return _rigidbody; }
        }

        private IsoTransform IsoTransform { get; set; }

        [ExposeProperty]
        public float Mass
        {
            get { return _mass; }
            set
            {
                _mass = Mathf.Clamp(value, 0, Mathf.Infinity);
                if (Rigidbody != null)
                    Rigidbody.mass = _mass;
            }
        }

        [ExposeProperty]
        public float Drag
        {
            get { return _drag; }
            set
            {
                _drag = value;
                if (Rigidbody != null)
                    Rigidbody.drag = value;
            }
        }

        [ExposeProperty]
        public bool UseGravity
        {
            get { return _useGravity; }
            set
            {
                _useGravity = value;
                if (Rigidbody != null)
                    Rigidbody.useGravity = value;
            }
        }

        [ExposeProperty]
        public bool IsKinematic
        {
            get { return _isKinematric; }
            set
            {
                _isKinematric = value;
                if (Rigidbody != null)
                    Rigidbody.isKinematic = value;
            }
        }

        [ExposeProperty]
        public RigidbodyInterpolation Interpolate
        {
            get { return _rigidbodyInterpolation; }
            set
            {
                _rigidbodyInterpolation = value;
                if (Rigidbody != null)
                    Rigidbody.interpolation = value;
            }
        }

        [ExposeProperty]
        public CollisionDetectionMode CollisionDetectionMode
        {
            get { return _collisionDetectionMode; }

            set
            {
                _collisionDetectionMode = value;
                if (Rigidbody != null)
                    Rigidbody.collisionDetectionMode = value;
            }
        }

        public Vector3 CenterOfMass
        {
            get { return Rigidbody ? Rigidbody.centerOfMass : Vector3.zero; }
            set
            {
                if (Rigidbody != null)
                    Rigidbody.centerOfMass = value;
            }
        }

        public Vector3 Velocity
        {
            get { return Rigidbody ? Rigidbody.velocity : Vector3.zero; }
            set
            {
                if (Rigidbody != null)
                    Rigidbody.velocity = value;
            }
        }

        public float MaxDepenetrationVelocity
        {
            get { return Rigidbody ? Rigidbody.maxDepenetrationVelocity : 0; }
            set
            {
                if (Rigidbody != null)
                    Rigidbody.maxDepenetrationVelocity = value;
            }
        }

        public float SleepThreshold
        {
            get { return Rigidbody ? Rigidbody.sleepThreshold : 0; }
            set
            {
                if (Rigidbody != null)
                    Rigidbody.sleepThreshold = value;
            }
        }

        public bool IsSleeping
        {
            get { return Rigidbody != null && Rigidbody.IsSleeping(); }
        }

        /// <summary>
        /// Adds a Rigidbody to the ghost
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetupRigidbody()
        {
            if (Rigidbody == null)
            {
                var ghostReference = transform.GetOrAddComponent<GhostReference>();
                ghostReference.hideFlags = HideFlags.HideInInspector;

                //waits until Awake has finished
                yield return new WaitUntil(() => ghostReference.GhostObject != null);
                _rigidbody = ghostReference.GhostObject.AddComponent<Rigidbody>();
                _rigidbody.freezeRotation = true;
                _rigidbody.isKinematic = IsKinematic;
                _rigidbody.useGravity = UseGravity;
            }
        }

        public void AddExplosionForce(float explosionForce, Vector3 explosionPosition,
            float explosionRadius, float upwardsModifier = 0.0F, ForceMode mode = ForceMode.Force)
        {
            Rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, mode);
        }

        public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            Rigidbody.AddForce(force, mode);
        }

        public void AddForceAtPosition(Vector3 force, Vector3 position, ForceMode mode = ForceMode.Force)
        {
            Rigidbody.AddForceAtPosition(force, position, mode);
        }

        public void AddRelativeForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            Rigidbody.AddRelativeForce(force, mode);
        }

        public void Sleep()
        {
            Rigidbody.Sleep();
        }

        public void WakeUp()
        {
            Rigidbody.WakeUp();
        }

        public bool SweepTest(Vector3 direction, out IsoRaycastHit hitInfo, float maxDistance = Mathf.Infinity,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            RaycastHit hit;
            if (Rigidbody.SweepTest(direction, out hit, maxDistance, queryTriggerInteraction))
            {
                hitInfo = IsoRaycastHit.FromRaycastHit(hit);
                return true;
            }
            else
            {
                hitInfo = new IsoRaycastHit();
            }

            return false;
        }

        public IsoRaycastHit[] SweepTestAll(Vector3 direction, float maxDistance = Mathf.Infinity,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            var hits = Rigidbody.SweepTestAll(direction, maxDistance, queryTriggerInteraction);
            return hits.Select(h => IsoRaycastHit.FromRaycastHit(h)).ToArray();
        }

        #region Unity Events

        protected void Awake()
        {
            if (IsoTransform == null)
                IsoTransform = GetComponent<IsoTransform>();
            //setup rigidbody on ghost
            StartCoroutine(SetupRigidbody());
        }

        void OnEnable()
        {
            if (Rigidbody != null)
                Rigidbody.detectCollisions = true;
        }

        void OnDisable()
        {
            if (Rigidbody != null)
                Rigidbody.detectCollisions = false;
        }

        void OnDestroy()
        {
            if (Rigidbody != null)
            {
                //has no collider, so delete whole gameObject
                var ghostRef = GetComponent<GhostReference>();
                if (ghostRef != null)
                    ghostRef.RemoveRigidBody();
            }
        }

        #endregion
    }
}
