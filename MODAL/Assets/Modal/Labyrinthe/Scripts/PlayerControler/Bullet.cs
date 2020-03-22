using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Utils
{
    public class Bullet : MonoBehaviour
    {
        private new Rigidbody rigidbody;
        private new Collider collider;

        private bool shooted = false;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        }
        
        public Rigidbody GetRigidBody()
        {
            return rigidbody;
        }

        public Collider GetCollider()
        {
            return collider;
        }

        public void SetShooted()
        {
            shooted = true;
        }

        public bool Shooted()
        {
            return shooted;
        }

        public void EnableElements()
        {
            collider.isTrigger = false;

            //rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }

        public void DisableElements()
        {
            collider.isTrigger = true;

            //rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }

        public void EnableGravity()
        {
            rigidbody.useGravity = true;
        }
    }
}