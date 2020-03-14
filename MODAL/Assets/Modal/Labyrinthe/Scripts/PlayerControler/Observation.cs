using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Utils
{
    public class Observation : MonoBehaviour
    {
        public GameObject player;
        public TimeManager timeManager;

        private Animator animator;
        private new Rigidbody rigidbody;

        private void Awake()
        {
            animator = player.GetComponent<Animator>();
            rigidbody = player.GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetBool("Observe", true);
                rigidbody.useGravity = false;
                //timeManager.DoSlowDown();
                timeManager.secondsToPast(5f);
                Debug.Log("Done");
            }
        }
        /*
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                rigidbody.AddForce( -Physics.gravity * 0.5f); // againt gravity
            }

        }
        */
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetBool("Observe", false);
                rigidbody.useGravity = true;
            }
        }

        private void Update()
        {
            Vector3 playerPosition = player.transform.position;
            transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
        }

    }
}