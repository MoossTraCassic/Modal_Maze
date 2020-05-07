using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.PNJ
{
    public class PNJSMBRangeAttack : SceneLinkedSMB<PNJBehaviour>
    {
        public float growthTime = 2.0f;

        protected float m_GrowthTimer = 0;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);

            m_MonoBehaviour.RememberTargetPosition();
            m_MonoBehaviour.Shoot();
            // m_MonoBehaviour.rifle.LoadProjectile();

            // m_MonoBehaviour.rifle.loadedProjectile.transform.up = Vector3.up;
            // m_MonoBehaviour.rifle.loadedProjectile.transform.forward = m_MonoBehaviour.transform.forward;

            // m_GrowthTimer = 0.0f;
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
            /*
            m_GrowthTimer = Mathf.Clamp(m_GrowthTimer + Time.deltaTime, 0.0f, growthTime);
            
            if (m_MonoBehaviour.rifle.loadedProjectile != null)
            m_MonoBehaviour.rifle.loadedProjectile.transform.localScale =
                Vector3.one * (m_GrowthTimer / growthTime);     // !!! Attention
            */
        }
    }
}