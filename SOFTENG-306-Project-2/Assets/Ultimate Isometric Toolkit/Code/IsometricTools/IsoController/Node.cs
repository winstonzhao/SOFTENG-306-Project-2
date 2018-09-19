using UnityEngine;
using System.Collections;

[RequireComponent(typeof(IsoObject))]
[ExecuteInEditMode]
public class Node : MonoBehaviour {

    [SerializeField]
    private IsoObject obj;

    public Vector3 Position {
        get { return obj.Position; }
        private set { obj.Position = value; }
    }

    void Start() {
        obj = GetComponent<IsoObject>();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Isometric.isoProjection(obj.Position), .3f);
    }
}
