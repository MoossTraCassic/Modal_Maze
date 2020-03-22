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

        private void Start()
        {
            animator = player.GetComponent<Animator>();
            rigidbody = player.GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                print("entered");
                animator.SetBool("Observe", true);
                rigidbody.useGravity = false;
                timeManager.DoSlowDown();
                //timeManager.secondsToPast(5f);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetBool("Observe", false);
                rigidbody.useGravity = true;
                timeManager.UnDoSlowMotion();
            }
        }

        private void Update()
        {
            FollowPlayer();
        }

        void FollowPlayer()
        {
            Vector3 playerPosition = player.transform.position;
            transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
        }
    }
}