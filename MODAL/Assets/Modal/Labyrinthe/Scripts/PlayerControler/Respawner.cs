using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ModalFunctions.Utils;

namespace ModalFunctions.Controller
{
	public class Respawner : MonoBehaviour
	{
		public static Respawner Instance { get{return s_instance;} }

		protected static Respawner s_instance;

		public float menuSwitchTime = 2f;
		public UnityEvent OnRespawnStart;

		[SerializeField]
		private StartUI m_Menu;

		private IEnumerator m_respawn;
		

		void Start()
		{
			s_instance = this;
			if(m_Menu == null)m_Menu = FindObjectOfType<StartUI>();
		}

		public void Respawn()
		{
			if(m_respawn != null) StopCoroutine(m_respawn);
			m_respawn = RespawnRoutine();
			StartCoroutine(m_respawn);
		}


		protected IEnumerator RespawnRoutine()
		{
			yield return new WaitForSeconds(menuSwitchTime);

			OnRespawnStart.Invoke();
			yield return StartCoroutine(ScreenFader.FadeSceneOut(ScreenFader.FadeType.GameOver));

			m_Menu.ActivateMenu();

			yield return StartCoroutine(ScreenFader.FadeSceneIn());
		}
	}
}