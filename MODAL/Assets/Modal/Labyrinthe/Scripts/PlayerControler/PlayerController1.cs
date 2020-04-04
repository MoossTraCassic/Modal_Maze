using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModalFunctions.Utils;

namespace ModalFunctions.Controller
{
    public class PlayerController1 : MonoBehaviour
    {
        [Tooltip("Layer Representing Ground")]
        public LayerMask ground;

        [Tooltip("Distance from ground to be considered as landed")]
        public float groundDistance = 0.1f;
        public float JumpForce = 500f;
        public BulletManager bulletManager;
        public bool canGoInAir = false;

        private Animator animator;
        private new Rigidbody rigidbody;
        private float speedFactor = 0.5f;

        private float m_horizontal;
        private float m_vertical;
        private float m_goFire;
        private float m_fire;

        private bool fired = false;
        private bool running = false;
        private bool stopRunning = false;
        private bool grounded = true;
        private bool jump = false;
        private bool inFirePose = false;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            Move();
        }

        private void GroundMovement()
        {
            running = false;

            if (Input.GetButton("RightOne") && speedFactor != 1f)
            {
                speedFactor = speedFactor < 1 ? speedFactor += 0.05f : speedFactor = 1;
                running = true;
            }
            if (Input.GetButtonUp("RightOne"))
            {
                stopRunning = true;
            }
            if (stopRunning)
            {
                speedFactor = speedFactor > 0.5f ? speedFactor -= 0.05f : speedFactor = 0.5f;
                if (speedFactor == 0.5f)
                {
                    stopRunning = false;
                }
            }
        }

        private void JumpMovement()
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
            /*else
            {
                rigidbody.AddForce(Vector3.up * JumpForce);
                animator.SetTrigger("Jump");
            }*/
            else
            {
                if (jump && animator.GetCurrentAnimatorStateInfo(0).IsName("Motion"))
                {
                    // jump!
                    rigidbody.velocity = new Vector3(rigidbody.velocity.x, JumpForce, rigidbody.velocity.z);
                    animator.SetTrigger("Jump");
                    grounded = false;
                    animator.applyRootMotion = false;
                    //groundDistance = 0.1f;
                }
            }
        }
/*
        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (grounded && Time.deltaTime > 0)
            {
                Vector3 v = (animator.deltaPosition * 1.5f) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = rigidbody.velocity.y;
                rigidbody.velocity = v;
            }
        }
*/
        private void FallFromObserveState()
        {
            foreach (GameObject orbeClone in bulletManager.GetClones())
            {
                if (orbeClone != null)
                {
                    orbeClone.GetComponent<OrbeRotation>().Decelerate();
                }
            }
            animator.SetBool("Observe", false);

        }

        private IEnumerator AddForceToGrounded(float forceAmount)
        {
            while (!grounded)
            {
                Vector3 jumpDirection = transform.forward;
                rigidbody.AddForce(jumpDirection * forceAmount);

                yield return null;
            }
        }

        private void CheckGroundStatus()
        {
            /*if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down, groundDistance, ground))
            {
                animator.SetBool("Grounded", true);
            }
            else
            {
                animator.SetBool("Grounded", false);
                /*
                                Vector3 v = (animator.deltaPosition) / Time.deltaTime;

                                // we preserve the existing y part of the current velocity.
                                v.y = rigidbody.velocity.y;
                                rigidbody.velocity = v;
                /
                Vector3 jumpDirection = rigidbody.velocity.normalized;//animator.deltaPosition;//animator.velocity.normalized; // * (speedFactor - 0.5f) * 2f;
                rigidbody.AddForce(jumpDirection * JumpForce);
            }*/
            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down, groundDistance, ground))
            {

                animator.SetBool("Grounded", true);
                animator.applyRootMotion = true;
            }
            else
            {
                if (!canGoInAir)
                {
                    animator.SetBool("Grounded", false);
                    animator.applyRootMotion = false;
                }
            }
        }

        private void ResetSpeedForFire()
        {
            if (m_goFire >= 0.5f && animator.GetCurrentAnimatorStateInfo(0).IsName("Motion"))
            {
                animator.SetBool("GoFire", true);
                speedFactor = speedFactor < 1 ? speedFactor += 0.06f : speedFactor = 1;
            }
            if (m_goFire == 0f && !running)
            {
                animator.SetBool("GoFire", false);
                speedFactor = speedFactor > 0.5f ? speedFactor -= 0.05f : speedFactor = 0.5f;
            }
        }

        private void Move()
        {
            m_horizontal = Input.GetAxis("Horizontal");
            m_vertical = Input.GetAxis("Vertical");

            m_goFire = Input.GetAxis("Axis_9");
            m_fire = Input.GetAxis("Axis_10");

            grounded = animator.GetCurrentAnimatorStateInfo(0).IsName("Motion");
            jump = Input.GetButtonDown("Jump");
            inFirePose = animator.GetCurrentAnimatorStateInfo(0).IsName("FirePose");

            if (grounded)
            {
                GroundMovement();
            }

            else
            {
                // apply extra gravity from multiplier:
                Vector3 extraGravityForce = (Physics.gravity * 10f) - Physics.gravity;
                rigidbody.AddForce(extraGravityForce);

                //groundDistance = rigidbody.velocity.y < 0 ? 1f : 0.01f;
            }

            
            if (jump && grounded)
            {
                JumpMovement();
            }
            if(jump && animator.GetBool("Observe"))
            {
                FallFromObserveState();
            }

            CheckGroundStatus();


            ResetSpeedForFire();
            
            if (m_fire >= 0.8f && inFirePose)
            {
                FIre();
            }
            if(m_fire == 0f)
            {
                fired = false;
            }

            animator.SetFloat("WalkSpeed", m_vertical * speedFactor, 0.2f, Time.deltaTime);
            animator.SetFloat("TurnSpeed", m_horizontal * speedFactor, 0.2f, Time.deltaTime);

        }

        private void FIre()
        {
            if (!fired)
            {
                animator.SetTrigger("Fire");

                fired = true;
            }
        }

        /*
        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (grounded && Time.deltaTime > 0)
            {
                Vector3 v = (animator.deltaPosition) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = rigidbody.velocity.y;
                rigidbody.velocity = v;
            }
        }*/
    }
}