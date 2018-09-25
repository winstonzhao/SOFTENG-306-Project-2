using UnityEngine;
using System.Collections;

/// <summary>
/// Use this as an example for a controller in a isoworld with collisiondetection
/// </summary>
/// 
[RequireComponent(typeof(IsoCollider))]
public class AdvancedIsoObjectController : MonoBehaviour
{
    public float speed = 5;

    Animator animator;

    public float jumpspeed = .1f;

    private Transform ghostObject;

    void Start() 
    {
        ghostObject = gameObject.GetComponent<IsoCollider>().ghost.transform;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() 
    {
        ghostObject.Translate(new Vector3(Input.GetAxis("Vertical"),0, Input.GetAxis("Horizontal") * -1) * speed * Time.deltaTime);
    }

    void Update() 
    {
        animator.SetFloat("hSpeed", Input.GetAxis("Horizontal") * -1);
        animator.SetFloat("vSpeed", Input.GetAxis("Vertical"));

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
            animator.SetBool("walking", false);
        } else {
            animator.SetBool("walking", true);
        }
    
        if (Input.GetKeyDown(KeyCode.Space)) {
            ghostObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpspeed, ForceMode.Impulse);
        } 
    }
}
