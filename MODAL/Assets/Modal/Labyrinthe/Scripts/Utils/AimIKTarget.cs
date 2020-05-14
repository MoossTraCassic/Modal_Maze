using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using ModalFunctions.Controller;

namespace ModalFunctions.Utils
{
	public class AimIKTarget : MonoBehaviour
	{
		public Vector3 offset = new Vector3(0,-2,0);
		public Transform target;

		private AimIK m_aimIK;
		//private Animator m_animator;
		private bool m_target;

		private float m_setTime;

		void Start()
		{
			//m_animator = GetComponent<Animator>();

			m_aimIK = GetComponent<AimIK>();
			m_aimIK.enabled = false;
		} 

		void CheckFireStatus()
		{
			m_target = PlayerController.instance.p_inFirePose;//m_animator.GetBool("GoFire");
			//Debug.Log(m_target);
		}


		void LateUpdate()
		{

			m_aimIK.solver.IKPosition = target.position + offset;	
			CheckFireStatus();

			if(m_target)
			{			
				m_aimIK.enabled = true;

			}
			else
			{
				m_aimIK.enabled = false;
				//m_aimIK.solver.transform.LookAt(target.position + offset);
			}
		}
	}
}