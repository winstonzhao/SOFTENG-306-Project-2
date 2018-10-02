using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UltimateIsometricToolkit.physics;

namespace UltimateIsometricToolkit.controller
{
    /// <summary>
    /// Simple continuous movement with WSAD/Arrow Keys movement.
    /// Note: This is an exemplary implementation. You may vary inputs, speeds, etc.
    /// </summary>
    [RequireComponent(typeof(IsoTransform))]
    [AddComponentMenu("UIT/CharacterController/Simple Controller")]
    public class SimpleIsoObjectController : MonoBehaviour
    {
        public float Speed = 10;

        private IsoTransform IsoTransform;

        private Animator Animator;

        private GhostReference GhostReference;

        private void Start()
        {
            IsoTransform = GetComponent<IsoTransform>();

            Animator = GetComponent<Animator>();

            GhostReference = GetComponent<GhostReference>();

            // Disable auto updating position - fixes movement due to weird as code in this library 
            GhostReference.GhostObject.GetComponent<Ghost>().AutoUpdatePosition = false;
        }

        private float InputGetAxis(string name)
        {
            if (Toolbox.Instance.FocusManager.Focus != null)
            {
                return 0.0f;
            }

            return Input.GetAxis(name);
        }

        private void FixedUpdate()
        {
            var vertical = InputGetAxis("Vertical");
            var horizontal = -InputGetAxis("Horizontal");
            var movement = new Vector3(vertical, 0, horizontal);
            IsoTransform.Translate(movement * Time.deltaTime * Speed);
        }

        private void Update()
        {
            var vertical = InputGetAxis("Vertical");
            var horizontal = -InputGetAxis("Horizontal");

            Animator.SetFloat("hSpeed", horizontal);
            Animator.SetFloat("vSpeed", vertical);
            Animator.SetBool("vIdle", vertical == 0f);
            Animator.SetBool("hIdle", horizontal == 0f);
            Animator.SetBool("walking", horizontal != 0f || vertical != 0f);

            if (GhostReference != null)
            {
                var ghostObject = GhostReference.GhostObject;
                if (ghostObject != null)
                {
                    IsoTransform.Position = ghostObject.transform.position;
                }
            }
        }
    }
}
