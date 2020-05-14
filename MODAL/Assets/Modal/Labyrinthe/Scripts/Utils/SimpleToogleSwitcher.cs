using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ModalFunctions.Utils
{
	public class SimpleToogleSwitcher : MonoBehaviour
	{
		private Toggle m_toggle;

		void Start()
		{
			m_toggle = GetComponent<Toggle>();
			m_toggle.isOn = false;
		}

		public void Switch()
		{
			m_toggle.isOn = !m_toggle.isOn;
		}
	}
}