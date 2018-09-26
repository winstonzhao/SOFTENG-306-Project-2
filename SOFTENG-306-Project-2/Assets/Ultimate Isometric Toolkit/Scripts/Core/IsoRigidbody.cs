using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Ultimate_Isometric_Toolkit.Scripts.Core
{
    [RequireComponent(typeof(IsoTransform), typeof(IsoCollider))]
    public class IsoRigidbody : MonoBehaviour
    {
        //the object that actually handles collision detection
        [SerializeField] private Ghost ghost;

        private void Start()
        {
            var collider = gameObject.GetComponent<IsoCollider>();
            ghost = collider.ghost;
            Assert.IsTrue(ghost != null);
            ghost.gameObject.AddComponent<Rigidbody>().freezeRotation = true;
        }
    }
}
