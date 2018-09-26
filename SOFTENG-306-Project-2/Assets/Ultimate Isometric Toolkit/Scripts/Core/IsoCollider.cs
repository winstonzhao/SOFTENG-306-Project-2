using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Core
{
    [RequireComponent(typeof(IsoTransform))]
    public class IsoCollider : MonoBehaviour
    {
        public Ghost ghost;

        private new IsoTransform transform;

        private void Awake()
        {
            transform = this.GetOrAddComponent<IsoTransform>();

            var gameObj = new GameObject();
            ghost = gameObj.AddComponent<Ghost>();
            var collider = gameObj.AddComponent<BoxCollider>();
            gameObj.name = "Ghost_" + transform.name;
            // Additional spacing to prevent intersecting cube colliders - Unity default problem
            var size = transform.Size;
            collider.size = new Vector3(size.x, size.y, size.z);
            ghost.transform.position = new Vector3(transform.Position.x, transform.Position.y, transform.Position.z);
        }

        private void FixedUpdate()
        {
            transform.Position = new Vector3(
                ghost.transform.position.x,
                ghost.transform.position.y,
                ghost.transform.position.z
            );
        }
    }
}
