using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.PNJ
{
    public class PNJSMBRangeAttack : SceneLinkedSMB<PNJBehaviour>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);

            m_MonoBehaviour.RememberTargetPosition();
            m_MonoBehaviour.Shoot();
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
        }
    }
}