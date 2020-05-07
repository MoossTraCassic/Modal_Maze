using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModalFunctions.PNJ;

public class PNJSMBIdle_ : StateMachineBehaviour
{
    private PNJBehaviour m_MonoBehaviour;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //**base.OnSLStateEnter(animator, stateInfo, layerIndex);
        m_MonoBehaviour = animator.GetComponent<PNJBehaviour>();
        //if we came back to idle, we reset all trigger & bool, as we want to "restart" a cycle of looking, orienting, attacking

        animator.SetBool(PNJBehaviour.hashInPursuitParam, false);
        // animator.ResetTrigger(PNJBehaviour.hashMeleeAttack);
        animator.ResetTrigger(PNJBehaviour.hashRangeAttack);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(m_MonoBehaviour);
        if (m_MonoBehaviour == null) Debug.Log("Danger");
            /*
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
                /
                // else if (toTarget.sqrMagnitude < m_MonoBehaviour.rangeRange * m_MonoBehaviour.rangeRange)
                if (toTarget.sqrMagnitude < m_MonoBehaviour.rangeRange * m_MonoBehaviour.rangeRange)
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
            }*/
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
