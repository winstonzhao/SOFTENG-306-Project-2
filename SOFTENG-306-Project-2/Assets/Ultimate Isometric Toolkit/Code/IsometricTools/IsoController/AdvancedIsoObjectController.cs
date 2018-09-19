using UnityEngine;
using System.Collections;

/// <summary>
/// Use this as an example for a controller in a isoworld with collisiondetection
/// </summary>
/// 
[RequireComponent(typeof(IsoCollider))]
public class AdvancedIsoObjectController : MonoBehaviour {

    public float speed = 5;
    public float jumpspeed = .1f;

    private Transform ghostObject;

    void Start() {
        ghostObject = gameObject.GetComponent<IsoCollider>().ghost.transform;
    }
    void Update() {
        ghostObject.Translate(new Vector3(Input.GetAxis("Vertical"),0, Input.GetAxis("Horizontal") * -1) * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)) {
            ghostObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpspeed, ForceMode.Impulse);
        }
    }

	
}
