using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModalFunctions.DamageSystem;

namespace ModalFunctions.PNJ
{
    public class Ammo : Projectile
    {
 
        public float projectileSpeed;
        public int damageAmount = 1;
        public LayerMask damageMask;
        public float explosionRadius;    //field used for Test
        public float explosionTimer = 0.1f;
        public ParticleSystem explosionVFX;
 
        protected float m_SinceFired;

        protected Weapon m_Shooter;
        protected Rigidbody m_RigidBody;
        protected MeshCollider m_meshCollider;
        protected ParticleSystem m_VFXInstance;
        int m_EnvironmentLayer = -1;
 
        private void Awake()
        {
            m_EnvironmentLayer = 1 << LayerMask.NameToLayer("Ground");

            m_RigidBody = GetComponent<Rigidbody>();
            m_RigidBody.detectCollisions = false;

            m_meshCollider = GetComponent<MeshCollider>();
            m_meshCollider.convex = true;

            m_VFXInstance = Instantiate(explosionVFX);
            m_VFXInstance.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            m_RigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;    // Think about making it discrete
            m_RigidBody.isKinematic = true;
            m_SinceFired = 0.0f;
        }

        public override void Shot(Vector3 target, Weapon shooter)
        {
            m_RigidBody.isKinematic = false;

            m_Shooter = shooter;


            m_RigidBody.velocity = GetVelocity(target);
 
            m_RigidBody.detectCollisions = false;
            m_meshCollider.isTrigger = true;

            transform.forward = target - transform.position;
        }

        private void FixedUpdate()
        {
            m_SinceFired += Time.deltaTime;

            if (m_SinceFired > 0.1f)
            {
                //we only enable collision after half a second to get it time to clear the grenadier body 
                m_RigidBody.detectCollisions = true;
                m_meshCollider.isTrigger = false;
            }

            if (explosionTimer > 0 && m_SinceFired > explosionTimer)
            {
  
                PlayFireVFX();

                if (m_SinceFired > explosionTimer + 0.5f)
                {
                    StopFireVFX();
                    m_SinceFired = 0f;
                }
            }
        }

        public void PlayFireVFX()
        {
            Vector3 playPosition = transform.position;
 

            m_VFXInstance.gameObject.transform.position = playPosition;
 
            m_VFXInstance.time = 0.0f;
            m_VFXInstance.gameObject.SetActive(true);
            m_VFXInstance.Play(true);
        }

        void StopFireVFX()
        {
            m_VFXInstance.Stop(true);
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


 
            Damageable d = o.GetComponent<Damageable>();

            if (d != null)
                d.ApplyDamage(message);

            pool.Free(this);
        }

        protected virtual void OnCollisionEnter(Collision other)
        {

            if ((damageMask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
            {
                print("Touched");
                DamageCollided(other.collider);
                gameObject.SetActive(false);
            }
        }

        private Vector3 GetVelocity(Vector3 target)
        {
            Vector3 velocity = Vector3.zero;
            Vector3 toTarget = target - transform.position;
 

            velocity = toTarget;
  
            velocity.Normalize();
            velocity.y = 0.05f;   

            Debug.DrawRay(transform.position, velocity * 3.0f, Color.blue);

            velocity *= projectileSpeed;

            return velocity;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
#endif

    }
}