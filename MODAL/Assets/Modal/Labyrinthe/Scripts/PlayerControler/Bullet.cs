using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Utils
{
    public class Bullet : MonoBehaviour
    {
        private new Rigidbody rigidbody;
        private new Collider collider;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        }
    }
}