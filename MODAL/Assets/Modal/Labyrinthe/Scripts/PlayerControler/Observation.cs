using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModalFunctions.Controller;

namespace ModalFunctions.Utils
{
    public class Observation : MonoBehaviour
    {
        public GameObject player;
        public TimeManager timeManager;

        private Animator animator;
        private new Rigidbody rigidbody;
        private Vector3 center;
        private bool centered;

        [SerializeField]
        private float m_observationTime = 5f;

        private void Start()
        {
            animator = player.GetComponent<Animator>();
            rigidbody = player.GetComponent<Rigidbody>();
            center = transform.position;
            centered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
 
 
                animator.SetBool("Observe", true);
                rigidbody.useGravity = false; 
                timeManager.PassTime(m_observationTime);
             
            }
        }
 
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
 
                animator.SetBool("Observe", false);
                rigidbody.useGravity = true;
 
            }
        }

        private IEnumerator translatePlayer()
        {
            while((player.transform.position - center).magnitude > 0.05f)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, center, 0.02f);
                yield return null;
            }
            print("Player centerd");
        }

        private void Update()
        {
            if (animator.GetBool("Grounded"))
            {
                FollowPlayer(); 
            }
        }

        void FollowPlayer()
        {
            Vector3 playerPosition = player.transform.position;
            transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
        }
    }
}