using UnityEngine;
using System.Collections;

[RequireComponent(typeof(IsoObject))]
public class IsoCollider : MonoBehaviour {

    public Ghost ghost;

    public IsoObject isoObj;

    public Vector3 Position {
        get { return isoObj.Position; }
        private set { isoObj.Position = value;}
    }

    public Vector3 Size {
        get { return isoObj.Size; }
        private set { isoObj.Size = value; }
    }

    void Awake() {
        isoObj = MethodExtensionForMonoBehaviourTransform.GetOrAddComponent<IsoObject>(this);
        var go = new GameObject();
        ghost = go.AddComponent<Ghost>();
        var collider = go.AddComponent<BoxCollider>();
        go.name = "Ghost_" + gameObject.name;
		//additional spacing to prevent intersecting cubecolliders.( Unity default problem)
        collider.size = new Vector3(isoObj.Size.x, isoObj.Size.z, isoObj.Size.y) * 1.1f;
        ghost.transform.position = new Vector3(isoObj.Position.x, isoObj.Position.z, isoObj.Position.y);
        
    }


    void FixedUpdate() {
        isoObj.Position = new Vector3(ghost.transform.position.x, ghost.transform.position.z, ghost.transform.position.y);
    }
    
}
