using System.Collections.Generic;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.physics
{
    /// <summary>
    /// Equivalent of the Physics class in Unity
    /// </summary>
    public class IsoPhysics
    {
        /// <summary>
        /// Raycasts against all objects in isometric space. 
        /// Note: Objects must have an IsoCollider attached to raycast against
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="isoRaycastHit"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        public static bool Raycast(Vector3 origin, Vector3 direction, out IsoRaycastHit isoRaycastHit,
            float maxDistance = Mathf.Infinity, int layermask = Physics.DefaultRaycastLayers)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(origin, direction, out raycastHit, maxDistance, layermask))
            {
                isoRaycastHit = IsoRaycastHit.FromRaycastHit(raycastHit);
                return !isoRaycastHit.Equals(default(IsoRaycastHit));
            }

            isoRaycastHit = default(IsoRaycastHit);
            return false;
        }

        /// <summary>
        /// Raycasts against all objects in isometric space. 
        /// Note: Objects must have an IsoCollider attached to raycast against
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="isoRaycastHit"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        public static bool Raycast(Ray ray, out IsoRaycastHit isoRaycastHit, float maxDistance = Mathf.Infinity,
            int layermask = Physics.DefaultRaycastLayers)
        {
            return Raycast(ray.origin, ray.direction, out isoRaycastHit, maxDistance, layermask);
        }

        /// <summary>
        /// Raycasts against all IsoColliders. Returns IsoRaycastHits or null otherwise
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="maxDistance"></param>
        /// <param name="layerMask"></param>
        /// <param name="queryTriggerInteraction"></param>
        /// <returns></returns>
        public static IsoRaycastHit[] RaycastAll(Ray ray, float maxDistance = Mathf.Infinity,
            int layerMask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            var hits = Physics.RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);
            if (hits == null || hits.Length == 0)
                return null;
            var isoHits = new List<IsoRaycastHit>();
            for (var i = 0; i < hits.Length; i++)
            {
                var raycastHit = hits[i];
                var isoRaycastHit = IsoRaycastHit.FromRaycastHit(raycastHit);
                if (!isoRaycastHit.Equals(default(IsoRaycastHit)))
                    isoHits.Add(isoRaycastHit);
            }

            return isoHits.Count == 0 ? null : isoHits.ToArray();
        }

        /// <summary>
        /// Isometric equivalent of Physcis.Boxcast
        /// Note: Objects must have an IsoCollider attached to raycast against
        /// </summary>
        /// <param name="center"></param>
        /// <param name="halfExtents"></param>
        /// <param name="direction"></param>
        /// <param name="isoRaycastHit"></param>
        /// <param name="orientation"></param>
        /// <param name="maxDistance"></param>
        /// <param name="layerMask"></param>
        /// <param name="queryTriggerInteraction"></param>
        /// <returns></returns>
        public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction,
            out IsoRaycastHit isoRaycastHit,
            Quaternion orientation, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            RaycastHit hit;
            if (Physics.BoxCast(center, halfExtents, direction, out hit, orientation, maxDistance, layerMask,
                queryTriggerInteraction))
            {
                isoRaycastHit = IsoRaycastHit.FromRaycastHit(hit);
                return !isoRaycastHit.Equals(default(IsoRaycastHit));
            }

            isoRaycastHit = default(IsoRaycastHit);
            return false;
        }

        public static bool BoxCast(Ray ray, Vector3 halfExtents, out IsoRaycastHit isoRaycastHit,
            Quaternion orientation, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return BoxCast(ray.origin, halfExtents, ray.direction, out isoRaycastHit, orientation, maxDistance,
                layerMask, queryTriggerInteraction);
        }

        /// <summary>
        /// BoxCasts against all IsoColliders. Return IsoRaycastHits or null otherwise
        /// </summary>
        /// <param name="center"></param>
        /// <param name="halfExtents"></param>
        /// <param name="direction"></param>
        /// <param name="orientation"></param>
        /// <param name="maxDistance"></param>
        /// <param name="layermask"></param>
        /// <param name="queryTriggerInteraction"></param>
        /// <returns></returns>
        public static IsoRaycastHit[] BoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction,
            Quaternion orientation, float maxDistance = Mathf.Infinity,
            int layermask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            var hits = Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, layermask,
                queryTriggerInteraction);
            if (hits == null || hits.Length == 0)
                return null;
            var isoHits = new List<IsoRaycastHit>();
            for (var i = 0; i < hits.Length; i++)
            {
                var raycastHit = hits[i];
                var isoRaycastHit = IsoRaycastHit.FromRaycastHit(raycastHit);
                if (!isoRaycastHit.Equals(default(IsoRaycastHit)))
                    isoHits.Add(isoRaycastHit);
            }

            return isoHits.Count == 0 ? null : isoHits.ToArray();
        }
    }
}
