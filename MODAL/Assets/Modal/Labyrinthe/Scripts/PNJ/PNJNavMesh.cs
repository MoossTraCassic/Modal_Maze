using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ModalFunctions.PNJ
{
    public class PNJNavMesh : MonoBehaviour
    {
        public bool follow = false;
        public List<Transform> idleDestinations;

        [SerializeField]
        private Transform target;
        [SerializeField]
        private float timeOfWait = 5f;

        private NavMeshAgent m_meshAgent;
        private IEnumerator m_follow;
        private Animator m_animator;

        private bool m_randomDestSet = false;
        private bool m_waitRoutineTrigger = true;
        private bool m_followRoutineTrigger = true;
        private Transform m_idleDestination;
        private IEnumerator m_waitCoroutine;
        private int m_currentIndex = 0;


        private void Start()
        {
            m_meshAgent = GetComponent<NavMeshAgent>();
            m_animator = GetComponent<Animator>();
        }

        void IdleMovement()
        {
            if (!m_randomDestSet) 
            {
                print("NewSet");
                m_currentIndex = (m_currentIndex + 1) % idleDestinations.Count;
                m_idleDestination = idleDestinations[m_currentIndex];
                m_meshAgent.SetDestination(m_idleDestination.position);
                m_randomDestSet = true;
            }
            else
            {
                // m_animator.walk
                if ((transform.position - m_idleDestination.position).magnitude < 0.5f && m_waitRoutineTrigger)
                {
                    // m_animator.idle;
                    if (m_waitCoroutine != null) StopCoroutine(m_waitCoroutine);
                    m_waitCoroutine = ResetDestinationAfterTime(timeOfWait);
                    StartCoroutine(m_waitCoroutine);

                    m_waitRoutineTrigger = false;
                }
                if((transform.position - m_idleDestination.position).magnitude > 0.6f)
                {
                    m_waitRoutineTrigger = true;
                }
            }
        }

        IEnumerator ResetDestinationAfterTime(float waitTime)
        {
            
            yield return new WaitForSeconds(waitTime);
            m_randomDestSet = false;
            // print("rsd");
        }

        public void SetTarget(Transform obj)
        {
            target = obj;
        }

        private IEnumerator FollowTarget(Transform target)
        {

            // Vector3 previousTargetPosition = target.position;// new Vector3(float.PositiveInfinity, float.PositiveInfinity);
            float timeOfSet = float.PositiveInfinity;
            float distance = float.PositiveInfinity;

            while (distance > 30f)
            {
                // m_animator.SetFloat("Forw", 1f, 0.5f, Time.deltaTime);
                // float distance = Vector3.SqrMagnitude(previousTargetPosition - transform.position);

                distance = Vector3.SqrMagnitude(target.position - transform.position);
                timeOfSet += (1 / distance);// * Time.deltaTime;

                // print("Distance :" + distance);
                //print("TimeOfSet:" + timeOfSet);

                if (timeOfSet > 0.05f)
                {
                    print("SetTarget");
                    m_meshAgent.SetDestination(target.position);
                    // previousTargetPosition = target.position;
                    timeOfSet = 0f;
                }
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
            print("ended");
            // Attack();
        }

        void Attack()
        {
            m_animator.SetFloat("Forw", 0f);
            m_animator.SetFloat("AttackType", (float)Random.Range(0, 2));
            m_animator.SetTrigger("Attack");
        }

        void GroundMovement()
        {
            if (m_followRoutineTrigger)
            {
                if (m_follow != null)
                {
                    StopCoroutine(m_follow);
                }
                m_follow = FollowTarget(target);
                StartCoroutine(m_follow);
                //follow = false;
                m_followRoutineTrigger = false;
            }
        }

        void Update()
        {
            if (!follow)
            {
                if(m_follow != null) StopCoroutine(m_follow);
                
                print("patrol");
                IdleMovement();
            }
            else
            {
                GroundMovement();
                ////print("Follow Started");
            }

        }
    }
}