using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCGrenadier : MonoBehaviour
{

    public bool follow = false;

    [SerializeField]
    private Transform target;

    private NavMeshAgent m_meshAgent;
    private IEnumerator m_follow;
    private Animator m_animator;

    private void Start()
    {
        m_meshAgent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (follow)
        {
            print("Follow Started");
            if (m_follow != null)
            {
                StopCoroutine(m_follow);
            }
            m_follow = FollowTarget(target);
            StartCoroutine(m_follow);
            follow = false;
        }
        
    }

    public void SetTarget(Transform obj)
    {
        target = obj;
    }

    private IEnumerator FollowTarget(Transform target)
    {
        
        Vector3 previousTargetPosition = target.position;// new Vector3(float.PositiveInfinity, float.PositiveInfinity);
        float timeOfSet = float.PositiveInfinity;
        while (Vector3.SqrMagnitude(transform.position-target.position) > 30f)
        {
            m_animator.SetFloat("Forw", 1f, 0.5f, Time.deltaTime);
            float distance = Vector3.SqrMagnitude(previousTargetPosition - transform.position);
            timeOfSet += (1 / distance);// * Time.deltaTime;

            // print("Distance :" + distance);
            //print("TimeOfSet:" + timeOfSet);
            
            if (timeOfSet > 0.05f)
            {
                //print("SetTarget");
                m_meshAgent.SetDestination(target.position);
                previousTargetPosition = target.position;
                timeOfSet = 0f;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
        Attack();
    }

    void Attack()
    {
        m_animator.SetFloat("Forw", 0f);
        m_animator.SetFloat("AttackType",(float)Random.Range(0, 2));
        m_animator.SetTrigger("Attack");
    }

}
