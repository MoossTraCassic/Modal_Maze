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
                /*if (!centered)
                {
                    StartCoroutine(translatePlayer());
                    centered = true;
                }*/
                //rigidbody.AddForce(Vector3.up * 1000f);
                animator.SetBool("Observe", true);
                rigidbody.useGravity = false;
                //timeManager.DoSlowDown();
                timeManager.PassTime(m_observationTime);
             
            }
        }
        /*
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && timeManager.TimePassed())
            {
                //PlayerController.instance.ObserveTimeOver += timeManager.TimePassed;
                //PlayerController.instance.FallFromObserveState();
                //animator.SetBool("Observe", false);
                // timeManager.ResetTimePassed();
                // rigidbody.useGravity = true;
            }
        }
        */
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //PlayerController.instance.ObserveTimeOver -= timeManager.TimePassed;
                animator.SetBool("Observe", false);
                rigidbody.useGravity = true;
                //timeManager.UnDoSlowMotion();
                //timeManager.ResetTimePassed();
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

            
             //   centered = false;
             //   center = transform.position;
            }
        }

        void FollowPlayer()
        {
            Vector3 playerPosition = player.transform.position;
            transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
        }
    }
}