using UnityEngine;
using System.Collections;

[RequireComponent(typeof(IsoObject))]
public class Tile : MonoBehaviour {


    public bool canBeReached = true;

    //wrap isoObj.position. comfort only
    public Vector3 Position {
        get {
            var isoObj = this.GetOrAddComponent<IsoObject>();
            return isoObj.Position; 
        }
        set {
            var isoObj = this.GetOrAddComponent<IsoObject>();
            isoObj.Position = value; 
        }
    }

    public Vector3 Size {
        get {
            var isoObj = this.GetOrAddComponent<IsoObject>();
            return isoObj.Size;
        }
        set {
            var isoObj = this.GetOrAddComponent<IsoObject>();
            isoObj.Size = value;
        }
    }


 
	
}
