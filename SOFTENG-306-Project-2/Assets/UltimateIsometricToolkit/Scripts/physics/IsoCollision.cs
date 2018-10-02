﻿using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;

namespace UltimateIsometricToolkit.physics
{
    public class IsoCollision
    {
        /// <summary>
        /// The isoCollider we hit (Read Only).
        /// </summary>
        public IsoCollider isoCollider { get; private set; }

        /// <summary>
        /// The contact points generated by the physics engine.
        /// </summary>
        public ContactPoint[] contacts { get; private set; }

        /// <summary>
        /// The GameObject whose collider we are colliding with. (Read Only).
        /// </summary>
        public GameObject gameObject { get; private set; }

        /// <summary>
        ///  The total impulse applied to this contact pair to resolve the collision.
        /// </summary>
        public Vector3 impulse { get; private set; }

        /// <summary>
        ///  The relative linear velocity of the two colliding objects (Read Only).
        /// </summary>
        public Vector3 relativeVelocity { get; private set; }

        /// <summary>
        /// The Rigidbody we hit (Read Only). This is null if the object we hit is a
        ///  collider with no rigidbody attached.
        /// </summary>
        public IsoRigidbody isoRigidbody { get; private set; }

        /// <summary>
        /// The IsoTransform of the object we hit (Read Only).
        /// </summary>
        public IsoTransform isoTransform { get; private set; }

        public IsoCollision(Collision collisionInfo)
        {
            var ghost = collisionInfo.collider.GetComponent<Ghost>();
            if (ghost != null)
            {
                isoCollider = ghost.IsoTransform.GetComponent<IsoCollider>();
                contacts = collisionInfo.contacts;
                gameObject = ghost.IsoTransform.gameObject;
                impulse = collisionInfo.impulse;
                relativeVelocity = collisionInfo.relativeVelocity;
                isoRigidbody = gameObject.GetComponent<IsoRigidbody>();
                isoTransform = ghost.IsoTransform;
            }
        }
    }
}
