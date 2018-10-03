using System.Collections;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.External;
using UnityEngine;

namespace UltimateIsometricToolkit.physics
{
    /// <summary>
    /// Abstract non-atachable isometric collider
    /// </summary>
    [RequireComponent(typeof(IsoTransform)), DisallowMultipleComponent]
    public abstract class IsoCollider : MonoBehaviour
    {
        /// <summary>
        /// Adds the correct Collider to the ghost obj
        /// </summary>
        /// <param name="obj">ghost obj</param>
        /// <returns></returns>
        protected abstract Collider instantiateCollider(GameObject obj);

        /// <summary>
        /// Draws the bounds in the Scene View using Gizmos
        /// </summary>
        public virtual void Draw()
        {
            InitForDraw();
        } //draw the bounds

        [HideInInspector, SerializeField]
        private IsoTransform _isoTransform;
        [HideInInspector, SerializeField]
        protected bool _isTrigger;
        [HideInInspector, SerializeField]
        protected PhysicMaterial _material;

        protected Collider Collider { get; set; }

        protected IsoTransform IsoTransform
        {
            get { return _isoTransform; }
            set { _isoTransform = value; }
        }
        [ExposeProperty]
        public bool IsTrigger
        {
            get { return _isTrigger; }
            set
            {
                _isTrigger = value;
                if (Collider != null)
                    Collider.isTrigger = value;
            }
        }

        [ExposeProperty]
        public PhysicMaterial Material
        {
            get { return _material; }
            set
            {
                _material = value;
                if (Collider != null)
                    Collider.material = value;
            }
        }

        /// <summary>
        /// Adds the correct collider to the ghost
        /// </summary>
        private IEnumerator SetupCollider()
        {
            if (Collider == null)
            {
                var ghostReference = transform.GetOrAddComponent<GhostReference>();
                ghostReference.hideFlags = HideFlags.HideInInspector;
                //waits Ghost was instantiated
                yield return new WaitUntil(() => ghostReference.GhostObject != null);
                Collider = instantiateCollider(ghostReference.GhostObject);
                Collider.isTrigger = _isTrigger;
                Collider.material = _material;
            }
        }

        /// <summary>
        /// Initializes the all fields before Awake for drawing purposes
        /// </summary>
        protected virtual void InitForDraw()
        {
            if (IsoTransform == null)
                IsoTransform = GetComponent<IsoTransform>();
        }

        #region  Unity Events

        void Awake()
        {
            if (IsoTransform == null)
                IsoTransform = GetComponent<IsoTransform>();
            StartCoroutine(SetupCollider());
        }

        void OnEnable()
        {
            if (Collider != null)
                Collider.enabled = true;
            IsoTransform = GetComponent<IsoTransform>();
        }

        void OnDisable()
        {
            if (Collider != null)
                Collider.enabled = false;
            IsoTransform = GetComponent<IsoTransform>();
        }

        void OnDestroy()
        {
            if (Collider != null)
            {
                //object has no Rigidbody
                var ghostRef = GetComponent<GhostReference>();
                if (ghostRef != null)
                    ghostRef.RemoveCollider();
            }
        }

        #endregion
    }
}
