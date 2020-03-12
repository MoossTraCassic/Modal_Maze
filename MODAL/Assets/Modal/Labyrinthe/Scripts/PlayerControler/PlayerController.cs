using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [Tooltip("Layer Representing Ground")]
        public LayerMask ground;

        [Tooltip("Distance from ground to be considered as landed")]
        public float groundDistance = 1f;
        public float JumpForce = 500f;

        private Animator animator;
        private Rigidbody rigidbody;
        private float speedFactor = 0.5f;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(Input.GetAxis("Axis_9"));
            Move();
        }

        private void Move()
        {

            // Handle walking movement
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            if (Input.GetButton("RightOne") && speedFactor != 1f) 
            {
                speedFactor = speedFactor < 1 ? speedFactor += 0.05f : speedFactor = 1;
            }
            if(Input.GetButtonUp("RightOne"))
            {
                speedFactor = 0.5f;
            }

            animator.SetFloat("WalkSpeed", vertical * speedFactor);
            animator.SetFloat("TurnSpeed", horizontal * speedFactor);

            // Handle Jumping
            if (Input.GetButtonDown("Jump") && animator.GetCurrentAnimatorStateInfo(0).IsName("Motion"))
            {
                rigidbody.AddForce(Vector3.up * JumpForce);
                animator.SetTrigger("Jump");
            }

            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down, groundDistance, ground))
            {
                animator.SetBool("Grounded", true);
            }
            else
            {
                animator.SetBool("Grounded", false);
                Vector3 jumpDirection = transform.forward * (speedFactor - 0.5f) * 2f;
                //jumpDirection = JumpForce * rigidbody.velocity.normalized;
                rigidbody.AddForce(jumpDirection * JumpForce);
            }

            // Handle Fire
            if (Input.GetButtonDown("LeftTwo") && animator.GetCurrentAnimatorStateInfo(0).IsName("Motion"))
            {
                animator.SetBool("GoFire", true);
                //rigidbody.AddTorque(transform.up * 1000f * horizontal);

            }
            if (Input.GetButton("LeftTwo") && speedFactor != 1f)
            {
                speedFactor = speedFactor < 1 ? speedFactor += 0.05f : speedFactor = 1;
            }
            if (Input.GetButtonUp("LeftTwo"))
            {
                animator.SetBool("GoFire", false);
                speedFactor = 0.5f;
            }

        }
    }
}