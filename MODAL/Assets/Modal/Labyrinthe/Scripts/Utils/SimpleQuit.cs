using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Utils
{
	public class SimpleQuit : MonoBehaviour
	{
		public void QuitGame()
		{
			Application.Quit();
			Debug.Log("Quit");
		}
	}
}
