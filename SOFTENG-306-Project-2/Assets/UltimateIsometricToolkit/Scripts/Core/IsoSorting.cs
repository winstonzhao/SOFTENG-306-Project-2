using System;
using Ultimate_Isometric_Toolkit.Scripts.External;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Core
{
    /// <summary>
    /// Wrapper class for the current sorting strategy
    /// </summary>
    [ExecuteInEditMode, AddComponentMenu("UIT/Sorting/IsoSorting")]
    public class IsoSorting : MonoBehaviour
    {
        public SortingStrategy SortingStrategy;
        [HideInInspector]
        public bool Dirty = true;

        [Obsolete("Deprecated, use the projection and xRot,yRot instead"), SerializeField, HideInInspector]
        private float _isoAngle = 26.565f;

        [SerializeField, HideInInspector]
        private float _xRot = 35.625f;

        [SerializeField, HideInInspector]
        private float _yRot = 45;

        private static IsoSorting _instance;

        [SerializeField, HideInInspector]
        private Isometric.Projection _projection = Isometric.Projection.Dimetric1To2;

        [SerializeField, HideInInspector]
        private IsoTransform _mouseOverIsoTransform;

        [Obsolete("Deprecated, use the projection and xRot,yRot instead")]
        public float IsoAngle
        {
            get { return _isoAngle; }
            set
            {
                value = Mathf.Clamp(value, 0, 45);
                _isoAngle = value;
                var isoAngleInRad = Mathf.Deg2Rad * _isoAngle;
                var arcsintan = Mathf.Asin(Mathf.Tan(isoAngleInRad)) * Mathf.Rad2Deg;
                Isometric.IsoMatrix = Isometric.GetProjectionMatrix(_projection, arcsintan, 45);
#if UNITY_EDITOR
                foreach (var isoTransform in FindObjectsOfType<IsoTransform>())
                {
                    Resolve(isoTransform);
                }
#endif
            }
        }

        [ExposeProperty]
        public Isometric.Projection Projection
        {
            get { return _projection; }
            set
            {
                _projection = value;
                Isometric.IsoMatrix = Isometric.GetProjectionMatrix(value, XRot, YRot);
#if UNITY_EDITOR
                foreach (var isoTransform in FindObjectsOfType<IsoTransform>())
                {
                    Resolve(isoTransform);
                }
#endif
            }
        }

        public float XRot
        {
            get { return _xRot; }
            set { _xRot = Mathf.Clamp(value, 0, 90); }
        }

        public float YRot
        {
            get { return _yRot; }
            set { _yRot = Mathf.Clamp(value, 0, 90); }
        }

        #region Unity callbacks

        void Awake()
        {
            if (SortingStrategy == null)
                SortingStrategy = FindObjectOfType<SortingStrategy>();
        }

        void OnEnable()
        {
            Isometric.IsoMatrix = Isometric.GetProjectionMatrix(Projection, XRot, YRot);
#if UNITY_EDITOR
            foreach (var isoTransform in FindObjectsOfType<IsoTransform>())
            {
                Resolve(isoTransform);
            }
#endif
        }

        void OnDisable()
        {
            Isometric.IsoMatrix = Isometric.GetProjectionMatrix(Projection, XRot, YRot);
#if UNITY_EDITOR
            foreach (var isoTransform in FindObjectsOfType<IsoTransform>())
            {
                Resolve(isoTransform);
            }
#endif
        }

        public void Update()
        {
            if (Camera.main != null)
                UpdateOnMouseCallbacks();
            if (!Dirty)
                return;
            if (SortingStrategy != null)
                SortingStrategy.Sort();
            Dirty = false;
        }

        #endregion

        private void UpdateOnMouseCallbacks()
        {
            //raycast
            IsoRaycastHit hit;
            if (IsoPhysics.Raycast(Isometric.MouseToIsoRay(), out hit))
            {
                if (_mouseOverIsoTransform != null)
                {
                    if (hit.IsoTransform != _mouseOverIsoTransform)
                    {
                        _mouseOverIsoTransform.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
                        hit.IsoTransform.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
                        _mouseOverIsoTransform.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
                    }
                }
                else
                {
                    hit.IsoTransform.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
                }

                _mouseOverIsoTransform = hit.IsoTransform;
                if (Input.GetMouseButtonDown(0))
                {
                    _mouseOverIsoTransform.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    _mouseOverIsoTransform.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                if (_mouseOverIsoTransform != null)
                {
                    _mouseOverIsoTransform.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
                    if (Input.GetMouseButtonDown(0))
                    {
                        _mouseOverIsoTransform.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        _mouseOverIsoTransform.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
                    }
                }

                _mouseOverIsoTransform = null;
            }
        }

        public void Resolve(IsoTransform isoTransform)
        {
            Dirty = true;
            if (SortingStrategy != null)
                SortingStrategy.Resolve(isoTransform);
        }

        public void Remove(IsoTransform isoTransform)
        {
            if (SortingStrategy != null)
                SortingStrategy.Remove(isoTransform);
        }
    }
}
