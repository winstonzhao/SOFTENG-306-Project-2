﻿using System;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;
using UnityEngine.Profiling;

namespace UltimateIsometricToolkit.physics
{
    /// <summary>
    /// Isometric ghost object that mimics the IsoTransform's position and vice versa
    /// </summary>
    [ExecuteInEditMode]
    public class Ghost : MonoBehaviour
    {
        [SerializeField]
        Vector3 _lastPos;

        [SerializeField]
        private Vector3 _offset;

        [SerializeField]
        public IsoTransform _isoTransform;

        [NonSerialized]
        public bool AutoUpdatePosition = true;

        public IsoTransform IsoTransform
        {
            get { return _isoTransform; }
            private set
            {
                if (value != null)
                    _isoTransform = value;
            }
        }

        [Obsolete("removed for performance reasons")]
        public Vector3 Offset
        {
            get { return _offset; }
            set
            {
                _offset = value;
//				UpdatePosition();
            }
        }

        public void Setup(IsoTransform isoTransform)
        {
            if (isoTransform)
            {
                IsoTransform = isoTransform;
                _lastPos = isoTransform.Position;
                transform.position = IsoTransform.Position;
                UpdateLayer();
            }
        }

        public void Move(Vector3 movement)
        {
            if (gameObject.isStatic)
            {
                return;
            }

            transform.Translate(movement);
        }

#if UNITY_EDITOR
#endif
        private void UpdatePosition()
        {
            if (_isoTransform != null)
            {
                // If the ghost position is not what we set it to, then it must have hit something
                if (transform.position != _lastPos)
                {
                    // Apply the collision detection to the iso transform
                    _isoTransform.Position = transform.position;
                    _lastPos = transform.position;
                }
                else
                {
                    // Tell the ghost to try moving to the iso transform position; see if it hits anything
                    transform.position = _isoTransform.Position;
                    _lastPos = _isoTransform.Position;
                }
            }
        }

        private void UpdateLayer()
        {
            if (_isoTransform != null)
            {
                gameObject.layer = _isoTransform.gameObject.layer;
            }
        }

        #region  Unity Events

        void Update()
        {
            UpdateLayer();
            gameObject.isStatic = _isoTransform.gameObject.isStatic;
            if (AutoUpdatePosition && !gameObject.isStatic)
            {
                UpdatePosition();
            }
        }

        //forwards Triggers and Collision to the IsoCollider
        void OnTriggerEnter(Collider other)
        {
            var otherghost = other.GetComponent<Ghost>();

            if (otherghost != null)
            {
                var isoCollider = otherghost.IsoTransform.GetComponent<IsoCollider>();
                IsoTransform.SendMessage("OnIsoTriggerEnter", isoCollider, SendMessageOptions.DontRequireReceiver);
            }
        }

        void OnTriggerExit(Collider other)
        {
            var otherghost = other.GetComponent<Ghost>();
            if (otherghost != null)
            {
                var isoCollider = otherghost.IsoTransform.GetComponent<IsoCollider>();
                IsoTransform.SendMessage("OnIsoTriggerExit", isoCollider, SendMessageOptions.DontRequireReceiver);
            }
        }

        void OnTriggerStay(Collider other)
        {
            var otherghost = other.GetComponent<Ghost>();
            if (otherghost != null)
            {
                var isoCollider = otherghost.IsoTransform.GetComponent<IsoCollider>();
                IsoTransform.SendMessage("OnIsoTriggerStay", isoCollider, SendMessageOptions.DontRequireReceiver);
            }
        }

        void OnCollisionEnter(Collision collisionInfo)
        {
            var isoCollision = new IsoCollision(collisionInfo);
            if (isoCollision.gameObject != null)
                IsoTransform.SendMessage("OnIsoCollisionEnter", isoCollision, SendMessageOptions.DontRequireReceiver);
        }

        void OnCollisionExit(Collision collisionInfo)
        {
            var isoCollision = new IsoCollision(collisionInfo);
            if (isoCollision.gameObject != null)
                IsoTransform.SendMessage("OnIsoCollisionExit", isoCollision, SendMessageOptions.DontRequireReceiver);
        }

        void OnCollisionStay(Collision collisionInfo)
        {
            var isoCollision = new IsoCollision(collisionInfo);
            if (isoCollision.gameObject != null)
                IsoTransform.SendMessage("OnIsoCollisionStay", isoCollision, SendMessageOptions.DontRequireReceiver);
        }

        #endregion
    }
}
