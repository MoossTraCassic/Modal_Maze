using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModalFunctions.Controller;
using ModalFunctions.DamageSystem;

namespace ModalFunctions.PNJ
{
    public class PNJSMBIdle : SceneLinkedSMB<PNJBehaviour>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);

            //if we came back to idle, we reset all trigger & bool, as we want to "restart" a cycle of looking, orienting, attacking
            
            animator.SetBool(PNJBehaviour.hashInPursuitParam, false);
            // animator.ResetTrigger(PNJBehaviour.hashMeleeAttack);
            animator.ResetTrigger(PNJBehaviour.hashRangeAttack);
            
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
            
            m_MonoBehaviour.FindTarget();
            if (m_MonoBehaviour.target != null)
            {
                Vector3 toTarget = m_MonoBehaviour.target.transform.position - m_MonoBehaviour.transform.position;
                /*
                if (toTarget.sqrMagnitude < m_MonoBehaviour.meleeRange * m_MonoBehaviour.meleeRange)
                {
                    if (m_MonoBehaviour.OrientTowardTarget() == GrenadierBehaviour.OrientationState.ORIENTED_FACE)
                    {
                        animator.SetTrigger(GrenadierBehaviour.hashMeleeAttack);
                    }
                    else if (!m_MonoBehaviour.shieldUp)
                    {
                        //we don't turn, we do a shield attack, reset the turn apram set by the orient function
                        animator.ResetTrigger(GrenadierBehaviour.hashTurnTriggerParam);
                        animator.SetTrigger(GrenadierBehaviour.hashRotateAttackParam);
                    }
                }
                */
                // else if (toTarget.sqrMagnitude < m_MonoBehaviour.rangeRange * m_MonoBehaviour.rangeRange)
                Damageable damagePlayer = m_MonoBehaviour.target.m_damageable;

                bool attackable = damagePlayer.currentHitPoints > 0;
                if (toTarget.sqrMagnitude < m_MonoBehaviour.rangeRange * m_MonoBehaviour.rangeRange && attackable)
                {
                    if (m_MonoBehaviour.OrientTowardTarget() != PNJBehaviour.OrientationState.IN_TRANSITION)
                    {
                        animator.SetTrigger(PNJBehaviour.hashRangeAttack);
                    }
                }
                else
                {
                    if (m_MonoBehaviour.OrientTowardTarget() != PNJBehaviour.OrientationState.IN_TRANSITION)
                    {
                        m_MonoBehaviour.StartPursuit();
                    }
                }
            }
        }
    }

}