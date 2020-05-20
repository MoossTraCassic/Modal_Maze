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
 
		private bool m_target;

		private float m_setTime;

		void Start()
		{
 

			m_aimIK = GetComponent<AimIK>();
			m_aimIK.enabled = false;
		} 

		void CheckFireStatus()
		{
			m_target = PlayerController.instance.p_inFirePose; 
 
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
 
			}
		}
	}
}