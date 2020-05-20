using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModalFunctions.Utils;
using ModalFunctions.DamageSystem;


namespace ModalFunctions.Controller
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;
        // public delegate bool EmptyHandle();
        // public event EmptyHandle ObserveTimeOver;

        [Tooltip("Layer Representing Ground")]
        public LayerMask ground;

        [Tooltip("Distance from ground to be considered as landed")]
        public float groundDistance = 0.1f;
        public float JumpForce = 500f;
        public BulletManager bulletManager;
        public TimeManager timeManager;
        
	    public Animator p_animator { get {return animator;} }
        public bool p_inFirePose { get{return PlayerIsInFireMode();} }
        public InventoryController Inventory { get { return inventoryController; } }
 
        [HideInInspector]
        public bool p_jump;
        public Damageable m_damageable{get;private set;}
        private PlayerInput m_Input;
        private InventoryController inventoryController;
        private Animator animator;
        private new Rigidbody rigidbody;
        private float speedFactor = 0.5f;
        [Range(1f, 500f)] [SerializeField] 
        float m_GravityMultiplier = 4f;

        private float m_horizontal;
        private float m_vertical;
        private float m_goFire;
        private float m_fire;
        private Vector3 m_cameraForward;
        private Vector3 m_desiredDirection;

        private bool fired = false;
 
        private bool grounded = true;
        private bool raycast_grounded = true;
        private bool jump = false;
        private bool inFirePose = false;
        private bool m_HasControl = true;
 
        [HideInInspector]
        public bool canGoInAir = false;
        [Range(0f,1f)]
        public float m_damp = 0.4f;

        public float m_angle = 180f;

        void Awake()
        {
            m_Input = GetComponent<PlayerInput>();
            inventoryController = GetComponent<InventoryController>();
            m_damageable = GetComponent<Damageable>();
 
            instance = this;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
 
            canGoInAir = false;
        }


        void Update()
        {
            Move();
        }
 

        private bool PlayerIsInFireMode()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("FirePose") ||
                   animator.GetCurrentAnimatorStateInfo(0).IsName("KyleFire");
        }

        private void GroundMovement()
        {
        
            
            if (Input.GetButtonDown("RightOne"))
            {
                speedFactor = 0.9f;
            }
            if (Input.GetButtonUp("RightOne"))
            {
                speedFactor = 0.5f;
            }
            
        }

        private void JumpMovement()
        {
            speedFactor = 0.5f;
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
                // Used for the initial vertical jump
                //** rigidbody.AddForce(Vector3.up * JumpForce);
                
                 rigidbody.velocity = new Vector3(rigidbody.velocity.x, JumpForce, rigidbody.velocity.z);
			     animator.applyRootMotion = false;
                

                animator.SetTrigger("Jump");
            }
        }
        public void FallFromObserveState(float smoothTranslate)
        {
            foreach (GameObject orbeClone in bulletManager.GetClones())
            {
                if (orbeClone != null)
                {
                    orbeClone.GetComponent<OrbeRotation>().Decelerate();
                }
            }
            StartCoroutine(AddForceToGrounded(smoothTranslate));
        }

        private IEnumerator AddForceToGrounded(float forceAmount)
        {
            while (!grounded)
            {
                Vector3 jumpDirection = transform.forward + 0.25f * transform.up ;
                rigidbody.AddForce(jumpDirection * forceAmount);
                forceAmount -= 0.025f * forceAmount;
                yield return null;
            }
            canGoInAir = false;
        }

        private void CheckGroundStatus()
        {
            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down, groundDistance, ground))
            {
                animator.SetBool("Grounded", true);

                animator.applyRootMotion = true; 
                 
            }
            else
            {
                animator.SetBool("Grounded", false);
                // apply extra gravity from multiplier:
                if (!canGoInAir)
                {
                    animator.applyRootMotion = false;
                    
                    Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
                    rigidbody.AddForce(extraGravityForce);

                }

                //raycast_grounded = false;
                /*
                                Vector3 v = (animator.deltaPosition) / Time.deltaTime;

                                // we preserve the existing y part of the current velocity.
                                v.y = rigidbody.velocity.y;
                                rigidbody.velocity = v;
                */
                // willl produce misbehavior if uncomment
                //Vector3 jumpDirection = transform.forward * (speedFactor - 0.5f); // * rigidbody.velocity.normalized.z * 8f;
                //rigidbody.AddForce(jumpDirection * JumpForce);
            }
        }

        private void ResetSpeedForFire()
        {
            if (m_goFire >= 0.5f && animator.GetCurrentAnimatorStateInfo(0).IsName("Motion"))
            {
                animator.SetBool("GoFire", true);
            }
            if (m_goFire < 0.5f )
            {
                animator.SetBool("GoFire", false); 
            }
        }

        private void Move()
        {
            m_HasControl = m_Input.HaveControl();

            m_horizontal = m_HasControl ? Input.GetAxis("Horizontal") : 0f;
            m_vertical = m_HasControl ? Input.GetAxis("Vertical") : 0f;

            m_vertical = m_HasControl ? Mathf.Clamp(m_vertical * speedFactor, 0f, 1f) : 0f;
            m_horizontal = m_HasControl ? Mathf.Clamp(m_horizontal, -1f, 1f) : 0f;

            m_cameraForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;  


            m_desiredDirection = m_vertical * m_cameraForward + m_horizontal * Camera.main.transform.right.normalized;
            float direction = Vector3.Angle(transform.forward, m_desiredDirection) * Mathf.Sign(Vector3.Dot(m_desiredDirection, transform.right))/m_angle;

            float speed = m_vertical < 0.1f ? m_vertical : m_desiredDirection.magnitude;

            animator.SetFloat("WalkSpeed", speed, 0.4f, Time.deltaTime);
            animator.SetFloat("TurnSpeed", direction, m_damp, Time.deltaTime);

            m_goFire = m_HasControl ? Input.GetAxis("Axis_9") : 0f;
            m_fire = m_HasControl ? Input.GetAxis("Axis_10") : 0f;

            grounded = animator.GetCurrentAnimatorStateInfo(0).IsName("Motion");
            jump = (Input.GetButtonDown("Jump") || p_jump) && m_HasControl ;
            inFirePose = animator.GetCurrentAnimatorStateInfo(0).IsName("FirePose");

            if (grounded)
            {
                GroundMovement();
            }

            if (jump && grounded)
            {
                JumpMovement();
            }
            if (jump && animator.GetBool("Observe") && !timeManager.TimePassed())
            {
                timeManager.ResetTimePassed();
                animator.SetBool("Observe", false);
                FallFromObserveState(500f);
            }
            if (timeManager.TimePassed())
            {
                FallFromObserveState(100f);
                animator.SetBool("Observe", false);
                timeManager.ResetTimePassed();
            }

            CheckGroundStatus();


            ResetSpeedForFire();

            if (m_fire >= 0.8f && inFirePose)
            {
                Fire();
            }
            if (m_fire < 0.8f)
            {
                fired = false;
            }
        }

        private void Fire()
        {
            if (!fired)
            {
                animator.SetTrigger("Fire");

                fired = true;
            }
        }

        public void Hit()
        {
            animator.SetTrigger("Hurt");
        }

        public void Die()
        {
            animator.SetTrigger("Death");
        }

    }
}
