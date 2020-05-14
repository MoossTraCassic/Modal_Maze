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
        /*public bool CanGoInAir
        {
            get { return canGoInAir; }
            set
            {
                canGoInAir = value;
            }
        }
        */
        public Damageable m_damageable{get;private set;}
        private PlayerInput m_Input;
        private InventoryController inventoryController;
        private Animator animator;
        private new Rigidbody rigidbody;
        private float speedFactor = 0.5f;
        [Range(1f, 50f)] [SerializeField] 
        float m_GravityMultiplier = 4f;

        private float m_horizontal;
        private float m_vertical;
        private float m_goFire;
        private float m_fire;

        private bool fired = false;
        //private bool running = false;
        //private bool stopRunning = false;
        private bool grounded = true;
        private bool raycast_grounded = true;
        private bool jump = false;
        private bool inFirePose = false;
        private bool m_HasControl = true;
        //private bool gravityByScript = false;
        [HideInInspector]
        public bool canGoInAir = false;

        void Awake()
        {
            m_Input = GetComponent<PlayerInput>();
            inventoryController = GetComponent<InventoryController>();
            m_damageable = GetComponent<Damageable>();
            //print("Player Founded");
            instance = this;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            
            /*
            if (rigidbody.isKinematic)
            {
                gravityByScript = true;
                //print(gravityByScript);
            }
            */
            canGoInAir = false;
        }


        void Update()
        {
            Move();
        }
        /*
        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (grounded && Time.deltaTime > 0)
            {
                Vector3 v = (animator.deltaPosition * 1f) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.x = rigidbody.velocity.x;
                v.z = rigidbody.velocity.z;

                rigidbody.velocity = v;
            }
        }
        */

        private bool PlayerIsInFireMode()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("FirePose") ||
                   animator.GetCurrentAnimatorStateInfo(0).IsName("KyleFire");
        }

        private void GroundMovement()
        {
            // running = false;
           /* 
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
            */
            
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
                rigidbody.AddForce(Vector3.up * JumpForce);

                // rigidbody.velocity = new Vector3(rigidbody.velocity.x, JumpForce, rigidbody.velocity.z);
				// m_IsGrounded = false;
				// animator.applyRootMotion = false;

                animator.SetTrigger("Jump");
            }
        }
        public void FallFromObserveState(float smoothTranslate)
        {
            // print("Start Addforce");
            foreach (GameObject orbeClone in bulletManager.GetClones())
            {
                if (orbeClone != null)
                {
                    orbeClone.GetComponent<OrbeRotation>().Decelerate();
                }
            }
            // animator.SetBool("Observe", false);
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
            // timeManager.ResetTimePassed();
            ///print("Landed");
        }

        private void CheckGroundStatus()
        {
            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down, groundDistance, ground))
            {
                animator.SetBool("Grounded", true);
                // animator.applyRootMotion = true;  //
                // raycast_grounded = true;
                // print("ground hitted");
            }
            else
            {
                animator.SetBool("Grounded", false);
                // apply extra gravity from multiplier:
                if (!canGoInAir)
                {
                    // animator.applyRootMotion = false;   //
                    
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
                // willl produce error if uncomment
                //Vector3 jumpDirection = transform.forward * (speedFactor - 0.5f); // * rigidbody.velocity.normalized.z * 8f;
                //rigidbody.AddForce(jumpDirection * JumpForce);
            }
        }

        private void ResetSpeedForFire()
        {
            if (m_goFire >= 0.5f && animator.GetCurrentAnimatorStateInfo(0).IsName("Motion"))
            {
                animator.SetBool("GoFire", true);
                //speedFactor = speedFactor < 1 ? speedFactor += 0.06f : speedFactor = 1;
            }
            if (m_goFire < 0.5f )//&& !running)
            {
                animator.SetBool("GoFire", false);
                //speedFactor = speedFactor > 0.5f ? speedFactor -= 0.05f : speedFactor = 0.5f;
            }
        }
        /*
        private void applyScriptGravity()
        {
            print("moving");
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0.1f, transform.position.z), 0.4f);
        }
        */

        private void Move()
        {
            m_HasControl = m_Input.HaveControl();

            m_horizontal = m_HasControl ? Input.GetAxis("Horizontal") : 0f;
            m_vertical = m_HasControl ? Input.GetAxis("Vertical") : 0f;

            m_vertical = m_HasControl ? Mathf.Clamp(m_vertical * speedFactor, 0f, 0.9f) : 0f;
            m_horizontal = m_HasControl ? Mathf.Clamp(m_horizontal, -0.85f, 0.85f) : 0f;

            animator.SetFloat("WalkSpeed", m_vertical, 0.4f, Time.deltaTime);
            animator.SetFloat("TurnSpeed", m_horizontal, 0.4f, Time.deltaTime);


            m_goFire = m_HasControl ? Input.GetAxis("Axis_9") : 0f;
            m_fire = m_HasControl ? Input.GetAxis("Axis_10") : 0f;

            grounded = animator.GetCurrentAnimatorStateInfo(0).IsName("Motion");
            jump = Input.GetButtonDown("Jump") && m_HasControl ;
            inFirePose = animator.GetCurrentAnimatorStateInfo(0).IsName("FirePose");
            /*
            if (gravityByScript && !raycast_grounded && !canGoInAir)
            {
                applyScriptGravity();   
            }
            */
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
                FallFromObserveState(500f);
            }
            if (timeManager.TimePassed())
            {
                FallFromObserveState(100f);
                timeManager.ResetTimePassed();
            }
          /*  if(ObserveTimeOver != null)
            {
                if (ObserveTimeOver())
                {
                    FallFromObserveState();
                }
            }*/

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
            /*
            m_vertical = Mathf.Clamp(m_vertical * speedFactor, 0f, 1f);
            m_horizontal = Mathf.Clamp(m_horizontal * speedFactor, -1f, 1f);

            animator.SetFloat("WalkSpeed", m_vertical, 0.5f, Time.deltaTime);
            animator.SetFloat("TurnSpeed", m_horizontal, 0.5f, Time.deltaTime);
            */
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
            // if(damageAudioPlayer != null) 
            //    damageAudioPlayer.PlayRandomClip();
            animator.SetTrigger("Hurt");
            // m_CoreMaterial.SetColor("_Color2", Color.red);
        }

        public void Die()
        {
            // if(deathAudioPlayer != null)
            //     deathAudioPlayer.PlayRandomClip();
            animator.SetTrigger("Death");
        }

    }
}

/*
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
*/
