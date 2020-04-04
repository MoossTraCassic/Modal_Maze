using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModalFunctions.Controller;

namespace ModalFunctions.Utils
{
    public class ItemInteraction : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().canGoInAir = true;
            }
        }
    }
}