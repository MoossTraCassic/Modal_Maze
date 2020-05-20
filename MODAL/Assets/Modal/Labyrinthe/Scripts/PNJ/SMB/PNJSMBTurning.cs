using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.PNJ
{
    public class PNJSMBTurning : SceneLinkedSMB<PNJBehaviour>
    {
        protected Vector3 originalForward;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.controller.applyAnimationRotation = true;
            originalForward = m_MonoBehaviour.transform.forward;

            base.OnSLStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
	    if(m_MonoBehaviour.target == null) return;

        Vector3 v = m_MonoBehaviour.transform.position - m_MonoBehaviour.target.transform.position;
        v.y = 0;

        float angle = Vector3.SignedAngle(originalForward, v, Vector3.up);

        animator.SetFloat(PNJBehaviour.hashTurnAngleParam, angle / 180.0f);
        
	    Vector3 newForward = new Vector3(originalForward.x + angle,0f,originalForward.z + angle);
	    m_MonoBehaviour.transform.forward = Vector3.Lerp(originalForward,newForward,Mathf.Abs(angle));
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.controller.applyAnimationRotation = true;

            base.OnSLStateExit(animator, stateInfo, layerIndex);
        }
    }	
}