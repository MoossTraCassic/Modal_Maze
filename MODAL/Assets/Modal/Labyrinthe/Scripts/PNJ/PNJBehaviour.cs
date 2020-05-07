using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ModalFunctions.Controller;
using ModalFunctions.DamageSystem;
using UnityEngine.Serialization;
using ModalFunctions.Audio;

namespace ModalFunctions.PNJ
{
    [RequireComponent(typeof(PNJController))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class PNJBehaviour : MonoBehaviour 
    {
        public enum OrientationState
        {
            IN_TRANSITION,
            ORIENTED_ABOVE,
            ORIENTED_FACE
        }
        
        public static readonly int hashInPursuitParam = Animator.StringToHash("InPursuit");
        public static readonly int hashSpeedParam = Animator.StringToHash("Speed");  // not set yet
        public static readonly int hashTurnAngleParam = Animator.StringToHash("Angle");
        public static readonly int hashTurnTriggerParam = Animator.StringToHash("TurnTrigger");
        // public static readonly int hashMeleeAttack = Animator.StringToHash("MeleeAttack");
        public static readonly int hashRangeAttack = Animator.StringToHash("RangeAttack");
        public static readonly int hashHitParam = Animator.StringToHash("Hit");
        public static readonly int hashDeathParam = Animator.StringToHash("Death");
        public static readonly int hashRotateAttackParam = Animator.StringToHash("Rotate");

        // Chomper
        public static readonly int hashSpotted = Animator.StringToHash("Spotted");
        public static readonly int hashNearBase = Animator.StringToHash("NearBase");

        public static readonly int hashIdleState = Animator.StringToHash("PNJIdle");
        

        public PNJController controller { get { return m_EnemyController; } }

        public TargetScanner playerScanner;

        // public float meleeRange = 4.0f;
        public float rangeRange = 10.0f;

        // public MeleeWeapon fistWeapon;
        public Weapon rifle;

        // public GameObject shield;

        // public SkinnedMeshRenderer coreRenderer;

        protected PNJController m_EnemyController;
        protected NavMeshAgent m_NavMeshAgent;

        // public bool shieldUp { get { return shield.activeSelf; } }

        public PlayerControllerTest target { get { return m_Target; } }
        public Damageable damageable { get { return m_Damageable; } }

        
        [Header("Audio")]
        public RandomAudioPlayer deathAudioPlayer;
        public RandomAudioPlayer damageAudioPlayer;
        public RandomAudioPlayer footstepAudioPlayer;
        public RandomAudioPlayer shootAudioPlayer;
        public RandomAudioPlayer spottedAudioPlayer;

        

        protected PlayerControllerTest m_Target;
        //used to store the position of the target when the TraptionGuard decide to shoot, so if the player
        //move between the start of the animation and the actual grenade launch, it shoot were it was not where it is now
        protected Vector3 m_AmmoTarget;
        // protected Material m_CoreMaterial;

        protected Damageable m_Damageable;
        // protected Color m_OriginalCoreMaterial;

        // protected float m_ShieldActivationTime;

        // From Chomper
        public TargetDistributor.TargetFollower followerData { get { return m_FollowerInstance; } }
        public Vector3 originalPosition { get; protected set; }
        [Tooltip("Time in seconde before the Chomper stop pursuing the player when the player is out of sight")]
        public float timeToStopPursuit;
        protected float m_TimerSinceLostTarget = 0.0f;
        protected TargetDistributor.TargetFollower m_FollowerInstance = null;

        //Test
        public PlayerControllerTest test;
        

        void OnEnable()
        {
            m_EnemyController = GetComponent<PNJController>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();

            SceneLinkedSMB<PNJBehaviour>.Initialise(m_EnemyController.animator, this);

            // fistWeapon.SetOwner(gameObject);
            // fistWeapon.EndAttack();

            // m_CoreMaterial = coreRenderer.materials[1];
            // m_OriginalCoreMaterial = m_CoreMaterial.GetColor("_Color2");

            m_EnemyController.animator.Play(hashIdleState, 0, Random.value);

            // shield.SetActive(false);

            m_Damageable = GetComponent<Damageable>();

            // From Chomper
            originalPosition = transform.position;
        }
        protected void OnDisable()
        {
            if (m_FollowerInstance != null)
                m_FollowerInstance.distributor.UnregisterFollower(m_FollowerInstance);
        }

        public void Spotted()
        {
            if (spottedAudioPlayer != null)
                 spottedAudioPlayer.PlayRandomClip();
        }

        /*
        private void Update()
        {
            
            if (m_ShieldActivationTime > 0)
            {
                m_ShieldActivationTime -= Time.deltaTime;

                if (m_ShieldActivationTime <= 0.0f)
                    DeactivateShield();
            }
            
        }
        */
        private void FixedUpdate()
        {
            
            //**m_Controller.animator.SetBool(hashGrounded, controller.grounded);

            Vector3 toBase = originalPosition - transform.position;
            toBase.y = 0;

            m_EnemyController.animator.SetBool(hashNearBase, toBase.sqrMagnitude < 0.25f * 0.25f);

            
        }

        public void FindTarget()
        {
            //m_Target = playerScanner.Detect(transform);
            
            // From Chomper
            //we ignore height difference if the target was already seen
            PlayerControllerTest target = playerScanner.Detect(transform);

            if (m_Target == null)
            {
                //we just saw the player for the first time, pick an empty spot to target around them
                if (target != null)
                {
                    m_EnemyController.animator.SetTrigger(hashSpotted);  
                    m_Target = target;
                    TargetDistributor distributor = target.GetComponentInChildren<TargetDistributor>();
                    if (distributor != null)
                        m_FollowerInstance = distributor.RegisterNewFollower();
                }
            }
            else
            {
                //we lost the target. But chomper have a special behaviour : they only loose the player scent if they move past their detection range
                //and they didn't see the player for a given time. Not if they move out of their detectionAngle. So we check that this is the case before removing the target
                if (target == null)
                {
                    m_TimerSinceLostTarget += Time.deltaTime;

                    if (m_TimerSinceLostTarget >= timeToStopPursuit)
                    {
                        Vector3 toTarget = m_Target.transform.position - transform.position;

                        if (toTarget.sqrMagnitude > playerScanner.detectionRadius * playerScanner.detectionRadius)
                        {
                            if (m_FollowerInstance != null)
                                m_FollowerInstance.distributor.UnregisterFollower(m_FollowerInstance);

                            //the target move out of range, reset the target
                            m_Target = null;
                        }
                    }
                }
                else
                {
                    if (target != m_Target)
                    {
                        if (m_FollowerInstance != null)
                            m_FollowerInstance.distributor.UnregisterFollower(m_FollowerInstance);

                        m_Target = target;

                        TargetDistributor distributor = target.GetComponentInChildren<TargetDistributor>();
                        if (distributor != null)
                            m_FollowerInstance = distributor.RegisterNewFollower();
                    }

                    m_TimerSinceLostTarget = 0.0f;
                }
            }
        
        }

        public void StartPursuit()
        {
            // m_EnemyController.animator.SetBool(hashInPursuitParam, true);

            // From Chomper
            if (m_FollowerInstance != null)
            {
                m_FollowerInstance.requireSlot = true;
                RequestTargetPosition();
            }

            m_EnemyController.animator.SetBool(hashInPursuitParam, true);
        }

        public void RequestTargetPosition()
        {
            Vector3 fromTarget = transform.position - m_Target.transform.position;
            fromTarget.y = 0;

            m_FollowerInstance.requiredPoint = m_Target.transform.position + fromTarget.normalized * rangeRange * 0.9f;
        }

        public void StopPursuit()
        {
            // m_EnemyController.animator.SetBool(hashInPursuitParam, false);

            // From Chomper
            if (m_FollowerInstance != null)
            {
                m_FollowerInstance.requireSlot = false;
            }

            m_EnemyController.animator.SetBool(hashInPursuitParam, false);
        }

        public void WalkBackToBase()
        {
            if (m_FollowerInstance != null)
                m_FollowerInstance.distributor.UnregisterFollower(m_FollowerInstance);
            m_Target = null;
            StopPursuit();
            m_EnemyController.SetTarget(originalPosition);
            m_EnemyController.SetFollowNavmeshAgent(true);
        }

        /*
        public void StartAttack()
        {
            fistWeapon.BeginAttack(true);
        }
        
        public void EndAttack()
        {
            fistWeapon.EndAttack();
        }
        */

        public void Hit()
        {
            if(damageAudioPlayer != null) 
                damageAudioPlayer.PlayRandomClip();
            m_EnemyController.animator.SetTrigger(hashHitParam);
            // m_CoreMaterial.SetColor("_Color2", Color.red);
        }

        public void Die()
        {
            if(deathAudioPlayer != null)
                deathAudioPlayer.PlayRandomClip();
            m_EnemyController.animator.SetTrigger(hashDeathParam);
        }

        /*
        public void ActivateShield()
        {
            shield.SetActive(true);
            m_ShieldActivationTime = 3.0f;
            m_Damageable.SetColliderState(false);
        }

        public void DeactivateShield()
        {
            shield.SetActive(false);
            m_Damageable.SetColliderState(true);
        }

        public void ReturnVulnerable()
        {
            m_CoreMaterial.SetColor("_Color2", m_OriginalCoreMaterial);
        }
        */

        public void RememberTargetPosition()
        {
            m_AmmoTarget = m_Target.transform.position;
        }

        public void PlayStep()
        {
            if(footstepAudioPlayer != null) 
                footstepAudioPlayer.PlayRandomClip();
        }

        public void Shoot()
        {
            if(shootAudioPlayer != null) 
                shootAudioPlayer.PlayRandomClip();

            Vector3 toTarget = m_AmmoTarget - transform.position;

            //the bullet is launched a couple of meters in "front" of the player, because it bounce and roll, to make it a bit ahrder for the player
            //to avoid it
            //**Vector3 target = transform.position + (toTarget - toTarget * 0.1f);
            Vector3 target = transform.position + (toTarget + toTarget * 0.1f);

            rifle.Attack(target);
        }

        public OrientationState OrientTowardTarget()
        {
            Vector3 v = m_Target.transform.position - transform.position;
            bool above = v.y > 0.3f;
            v.y = 0;

            float angle = Vector3.SignedAngle(transform.forward, v, Vector3.up);

            if (Mathf.Abs(angle) < 20.0f)
            { //for a very small angle, we directly rotate the model
                transform.forward = v.normalized;
                // if the player was above the player we return false to tell the Idle state 
                // that we want a "shield up" attack as our punch attack wouldn't reach it.
                return above ? OrientationState.ORIENTED_ABOVE : OrientationState.ORIENTED_FACE;
            }

            m_EnemyController.animator.SetFloat(hashTurnAngleParam, angle / 180.0f);
            m_EnemyController.animator.SetTrigger(hashTurnTriggerParam);
            return OrientationState.IN_TRANSITION;
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            playerScanner.EditorGizmo(transform);
        }
#endif
    }
}