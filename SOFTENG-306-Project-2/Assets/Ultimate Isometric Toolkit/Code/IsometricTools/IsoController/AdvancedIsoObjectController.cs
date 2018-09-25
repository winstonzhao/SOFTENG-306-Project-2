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
        ghostObject.Translate(new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal") * -1) * speed *
                              Time.deltaTime);
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        animator.SetFloat("hSpeed", horizontal * -1);
        animator.SetFloat("vSpeed", vertical);

        var hIdle = horizontal == 0f;
        var vIdle = vertical == 0f;

        animator.SetBool("hIdle", hIdle);
        animator.SetBool("vIdle", vIdle);
        animator.SetBool("walking", !hIdle || !vIdle);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ghostObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpspeed, ForceMode.Impulse);
        }
    }
}
