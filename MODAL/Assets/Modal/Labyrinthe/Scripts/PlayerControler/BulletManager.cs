﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Utils
{
    public class BulletManager : MonoBehaviour
    {
        [Tooltip("List of Orbes prefab")]
        public List<GameObject> orbes = new List<GameObject>();
        public Animator animator;
        [Tooltip("The parent of the clones orbes (must be Kyle)")]
        public Transform parent;
        [Tooltip("The center of OrbeRotation")]
        public Transform center;

        public Transform handPosition;

        private int activeBullet = 0;
        private List<GameObject> clones = new List<GameObject>();
        private bool rotationReset = true;
        private bool joined = false;

        void Start()
        {
            GameObject orbe0 = Instantiate<GameObject>(orbes[activeBullet], parent);
            orbe0.GetComponent<OrbeRotation>().center = center;
            clones.Add(orbe0);
            activeBullet++;

            GameObject orbe1 = Instantiate<GameObject>(orbes[activeBullet], parent);
            orbe1.GetComponent<OrbeRotation>().center = center;
            clones.Add(orbe1);
            activeBullet++;
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerIsInFirePose())
            {
                PrepareBullet();
            }

            if (!PlayerIsInFirePose() && !rotationReset)
            {
                foreach(GameObject clone in clones)
                {
                    if(clone != null)
                    {
                        clone.GetComponent<OrbeRotation>().resetRotation();
                    }
                }
                rotationReset = true;
                joined = false;
            }
        }

        void PrepareBullet()
        {
            GameObject currentOrbe = clones[activeBullet - 1];
            currentOrbe.GetComponent<OrbeRotation>().StopRotation();

            Bullet bullet = currentOrbe.GetComponentInChildren<Bullet>();
            if ((bullet.transform.position - handPosition.position).magnitude <= 0.01f) // animations to play here
            {
                joined = true;
               
                //bullet.transform.position = handPosition.position;
            }
            else
            {
                if (!joined)
                {
                    bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, handPosition.position, 0.8f);
                    bullet.transform.rotation = handPosition.rotation;
                }
            }
            rotationReset = false;
        }

        bool PlayerIsInFirePose()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("FirePose") ||
                   animator.GetCurrentAnimatorStateInfo(0).IsName("KyleFire");
        }
    }
}