using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

namespace ModalFunctions.Utils
{ 
    public class AimIKTarGetState : StateMachineBehaviour
    {
        public Vector3 offset = new Vector3(0,-2,0);
        public AimIK aimIK;
        public Transform target;


        /*void LateUpdate()
        {
            //aimIK.solver.transform.LookAt(target.position);

            aimIK.solver.IKPosition = target.position + offset;
        }*/
    
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            aimIK.solver.IKPosition = target.position + offset;
        }
    }
}