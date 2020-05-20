using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ModalFunctions.DamageSystem;

namespace ModalFunctions.PNJ
{
    public class PNJSMBPursuit : SceneLinkedSMB<PNJBehaviour>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);

            if (m_MonoBehaviour.target != null)
            {
                //This will trigger the turning animation if it need to reorient
                m_MonoBehaviour.OrientTowardTarget();
            }
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

            m_MonoBehaviour.FindTarget();

            if (m_MonoBehaviour.controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathPartial
                || m_MonoBehaviour.controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                m_MonoBehaviour.StopPursuit();
                return;
            }

            if (m_MonoBehaviour.target != null)
            {
               
                Vector3 targetPos = m_MonoBehaviour.target.transform.position;
                
                Vector3 toTarget = m_MonoBehaviour.target.transform.position - m_MonoBehaviour.transform.position;
                float distToTarget = toTarget.sqrMagnitude;

                
                m_MonoBehaviour.controller.SetTarget(targetPos);

                Damageable damagePlayer = m_MonoBehaviour.target.m_damageable;

                bool attackable = damagePlayer.currentHitPoints > 0;
               if (distToTarget <= m_MonoBehaviour.rangeRange * m_MonoBehaviour.rangeRange && attackable)
                {
                    m_MonoBehaviour.OrientTowardTarget();
                    animator.SetTrigger(PNJBehaviour.hashRangeAttack);
                    animator.SetBool(PNJBehaviour.hashInPursuitParam, false);   //TODO: Try with deselected
                }
             
            }
            else
            {
             m_MonoBehaviour.StopPursuit();
            }
        }
    }
}