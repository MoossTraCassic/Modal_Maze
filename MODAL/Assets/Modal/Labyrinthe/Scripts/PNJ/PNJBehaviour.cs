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

        public float rangeRange = 10.0f;

        public Weapon rifle;

        protected PNJController m_EnemyController;
        protected NavMeshAgent m_NavMeshAgent;

        public PlayerController target { get { return m_Target; } }
        public Damageable damageable { get { return m_Damageable; } }

        
        [Header("Audio")]
        public RandomAudioPlayer deathAudioPlayer;
        public RandomAudioPlayer damageAudioPlayer;
        public RandomAudioPlayer footstepAudioPlayer;
        public RandomAudioPlayer shootAudioPlayer;
        public RandomAudioPlayer spottedAudioPlayer;

        

        protected PlayerController m_Target;
        protected Vector3 m_AmmoTarget;

        protected Damageable m_Damageable;
        public TargetDistributor.TargetFollower followerData { get { return m_FollowerInstance; } }
        public Vector3 originalPosition { get; protected set; }
        [Tooltip("Time in seconde before the Chomper stop pursuing the player when the player is out of sight")]
        public float timeToStopPursuit;
        protected float m_TimerSinceLostTarget = 0.0f;
        protected TargetDistributor.TargetFollower m_FollowerInstance = null;

        

        void OnEnable()
        {
            m_EnemyController = GetComponent<PNJController>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();

            SceneLinkedSMB<PNJBehaviour>.Initialise(m_EnemyController.animator, this);

            m_EnemyController.animator.Play(hashIdleState, 0, Random.value);

            m_Damageable = GetComponent<Damageable>();

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


        private void FixedUpdate()
        {

            Vector3 toBase = originalPosition - transform.position;
            toBase.y = 0;

            m_EnemyController.animator.SetBool(hashNearBase, toBase.sqrMagnitude < 0.25f * 0.25f);

            
        }

        public void FindTarget()
        {
            m_Target = playerScanner.Detect(transform);
        }

        public void StartPursuit()
        {

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


        public void Hit()
        {
            if(damageAudioPlayer != null) 
                damageAudioPlayer.PlayRandomClip();
            m_EnemyController.animator.SetTrigger(hashHitParam);

        }

        public void Die()
        {
            if(deathAudioPlayer != null)
                deathAudioPlayer.PlayRandomClip();
            m_EnemyController.animator.SetTrigger(hashDeathParam);
        }

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

            Vector3 target = transform.position + (toTarget + Vector3.up * 0.1f);

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