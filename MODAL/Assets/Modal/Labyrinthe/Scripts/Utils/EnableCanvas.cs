using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCanvas : MonoBehaviour
{
    //public Canvas pointer;
	public  GameObject pointer;

    private void OnEnable()
    {
        pointer.SetActive(true);
    }

    private void OnDisable()
    {
        pointer.SetActive(false);
    }
}
