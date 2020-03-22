﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModalFunctions.Utils;

namespace ModalFunctions.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [Tooltip("Layer Representing Ground")]
        public LayerMask ground;

        [Tooltip("Distance from ground to be considered as landed")]
        public float groundDistance = 1f;
        public float JumpForce = 500f;
        public BulletManager bulletManager;
        public bool canGoInAir = false;

        private Animator animator;
        private new Rigidbody rigidbody;
        private float speedFactor = 0.5f;

        private bool fired = false;
        private bool stopRunning = false;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void Move()
        {

            // Handle walking movement
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            bool running = false;

            if (Input.GetButton("RightOne") && speedFactor != 1f && animator.GetCurrentAnimatorStateInfo(0).IsName("Motion")) 
            {
                speedFactor = speedFactor < 1 ? speedFactor += 0.05f : speedFactor = 1;
                running = true;
            }
            if(Input.GetButtonUp("RightOne"))
            {
                stopRunning = true;
            }
            if (stopRunning && animator.GetCurrentAnimatorStateInfo(0).IsName("Motion"))
            {
                speedFactor = speedFactor > 0.5f ? speedFactor -= 0.05f : speedFactor = 0.5f;
                if(speedFactor == 0.5f)
                {
                    //Debug.Log("Walk");
                    stopRunning = false;
                }
            }

            animator.SetFloat("WalkSpeed", vertical * speedFactor);
            animator.SetFloat("TurnSpeed", horizontal * speedFactor);

            // Handle Jumping and goInAir
            if (Input.GetButtonDown("Jump") && animator.GetCurrentAnimatorStateInfo(0).IsName("Motion"))
            {
                if (canGoInAir)
                {
                    foreach (GameObject orbeClone in bulletManager.GetClones())
                    {
                        if (orbeClone != null)
                        {
                            orbeClone.GetComponent<OrbeRotation>().Accelerate();
                        }
                    }
                    animator.SetTrigger("GoInObservation");
                }
                else
                {
                    rigidbody.AddForce(Vector3.up * JumpForce);
                    animator.SetTrigger("Jump");
                }
            }
            if(Input.GetButtonDown("Jump") && animator.GetBool("Observe"))
            {
                foreach (GameObject orbeClone in bulletManager.GetClones())
                {
                    if (orbeClone != null)
                    {
                        orbeClone.GetComponent<OrbeRotation>().Decelerate();
                    }
                }
                animator.SetBool("Observe", false);
                //rigidbody.AddForce(Vector3.forward * JumpForce * 10f);
            }

            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down, groundDistance, ground))
            {
                animator.SetBool("Grounded", true);
            }
            else
            {
                animator.SetBool("Grounded", false);
                Vector3 jumpDirection = transform.forward * (speedFactor - 0.5f) * 2f;
                rigidbody.AddForce(jumpDirection * JumpForce);
            }

            // Handle Go Fire Position
            var goFire = Input.GetAxis("Axis_9");
            var fire = Input.GetAxis("Axis_10");
            if (goFire >= 0.5f && animator.GetCurrentAnimatorStateInfo(0).IsName("Motion"))
            {
                animator.SetBool("GoFire", true);
                speedFactor = speedFactor < 1 ? speedFactor += 0.06f : speedFactor = 1;
            }
            if (goFire == 0f && !running )
            {
                animator.SetBool("GoFire", false);
                speedFactor = speedFactor > 0.5f ? speedFactor -= 0.05f : speedFactor = 0.5f;
            }

            // Handle Fire
            if (fire >= 0.8f && animator.GetCurrentAnimatorStateInfo(0).IsName("FirePose"))
            {
                if (!fired)
                {
                    animator.SetTrigger("Fire");

                    fired = true;
                }
            }
            if(fire == 0f)
            {
                fired = false;
            }

            // Handle Observation


        }
    }
}