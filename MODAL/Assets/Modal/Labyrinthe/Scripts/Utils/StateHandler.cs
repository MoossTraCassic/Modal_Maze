using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.PNJ
{
    public class StateHandler : SceneLinkedSMB<PNJBehaviour>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);

            //if we came back to idle, we reset all trigger & bool, as we want to "restart" a cycle of looking, orienting, attacking
            /*
            **animator.SetBool(GrenadierBehaviour.hashInPursuitParam, false);
            **animator.ResetTrigger(GrenadierBehaviour.hashMeleeAttack);
            **animator.ResetTrigger(GrenadierBehaviour.hashRangeAttack);
            */
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

            m_MonoBehaviour.FindTarget();
            if (m_MonoBehaviour.target != null)
            {
                Vector3 toTarget = m_MonoBehaviour.target.transform.position - m_MonoBehaviour.transform.position;
                
                if (toTarget.sqrMagnitude < m_MonoBehaviour.rangeRange * m_MonoBehaviour.rangeRange)
                {
                    if (m_MonoBehaviour.OrientTowardTarget() != PNJBehaviour.OrientationState.IN_TRANSITION)
                    {
                        //**animator.SetTrigger(GrenadierBehaviour.hashRangeAttack);
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