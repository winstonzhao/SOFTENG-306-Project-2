﻿using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UserInterface;

namespace Ultimate_Isometric_Toolkit.Scripts.IsoController
{
    /// <summary>
    /// Simple continuous movement with WSAD/Arrow Keys movement.
    /// Note: This is an exemplary implementation. You may vary inputs, speeds, etc.
    /// </summary>
    [AddComponentMenu("UIT/CharacterController/Simple Controller")]
    public class IsoObjectController : MonoBehaviour
    {
        public float Speed = 10;

        private Ghost ghost;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
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
            if (ghost == null)
            {
                ghost = GetComponent<IsoCollider>().ghost;
                Assert.IsTrue(ghost != null);
            }

            var vertical = InputGetAxis("Vertical");
            var horizontal = -InputGetAxis("Horizontal");
            var movement = new Vector3(vertical, 0, horizontal);
            ghost.transform.Translate(movement * Time.deltaTime * Speed);
        }

        private void Update()
        {
            var vertical = InputGetAxis("Vertical");
            var horizontal = -InputGetAxis("Horizontal");

            animator.SetFloat("hSpeed", horizontal);
            animator.SetFloat("vSpeed", vertical);
            animator.SetBool("vIdle", vertical == 0f);
            animator.SetBool("hIdle", horizontal == 0f);
            animator.SetBool("walking", horizontal != 0f || vertical != 0f);
        }
    }
}
