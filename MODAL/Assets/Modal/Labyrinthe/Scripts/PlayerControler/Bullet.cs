using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModalFunctions.DamageSystem;

namespace ModalFunctions.Utils
{
    public class Bullet : MonoBehaviour
    {
        private new Rigidbody rigidbody;
        private new Collider collider;

        public int damageAmount = 1;
        public LayerMask damageMask;

        private bool shooted = false;
        int m_EnvironmentLayer = -1;

        private void Start()
        {
            m_EnvironmentLayer = 1 << LayerMask.NameToLayer("Ground");

            rigidbody = GetComponent<Rigidbody>();
            // rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;  

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

        public void DamageCollided(Collider o)
        {

            Damageable.DamageMessage message = new Damageable.DamageMessage
            {
                amount = damageAmount,
                damageSource = transform.position,
                damager = this,
                stopCamera = false,
                throwing = true
            };


            // for (int i = 0; i < count; ++i)
            // {
            //     Damageable d = m_ExplosionHitCache[i].GetComponentInChildren<Damageable>();
            
            Damageable d = o.GetComponent<Damageable>();

            if (d != null)
                d.ApplyDamage(message);
            // }
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            // if (bouncePlayer != null)
            //    bouncePlayer.PlayRandomClip();

            if ((damageMask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
            {
                print("Touched PNJ");
                DamageCollided(other.collider);
            }
        }
    }
}