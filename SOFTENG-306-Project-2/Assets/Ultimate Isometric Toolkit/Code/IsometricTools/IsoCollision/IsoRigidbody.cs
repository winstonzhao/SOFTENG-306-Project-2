using UnityEngine;
using System.Collections;

[RequireComponent(typeof(IsoObject))]
public class IsoRigidbody : MonoBehaviour {

    //the object that actually handles collsion detection
    [SerializeField]
    private Ghost ghost;

    private IsoObject isoObj;

    private bool hasCollider;

    void Start() {
        isoObj = this.GetOrAddComponent<IsoObject>();
        var collider = gameObject.GetComponent<IsoCollider>();
        hasCollider = collider != null; 
        if (!hasCollider) {
            var go = new GameObject();
            ghost = go.AddComponent<Ghost>();
            go.AddComponent<Rigidbody>().freezeRotation = true;
            ghost.transform.position = isoObj.Position;
        } else {
            ghost = collider.ghost;
            ghost.gameObject.AddComponent<Rigidbody>().freezeRotation = true;
        }
    }


    void FixedUpdate() {
        if(!hasCollider)
            isoObj.Position = ghost.transform.position;
    }
    

}
