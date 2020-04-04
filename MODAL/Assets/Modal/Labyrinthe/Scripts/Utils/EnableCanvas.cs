using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCanvas : MonoBehaviour
{
    public Canvas pointer;

    private void OnEnable()
    {
        pointer.enabled = true;
    }

    private void OnDisable()
    {
        pointer.enabled = false;
    }
}
