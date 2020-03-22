using System.Collections;
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

        public float waitTime = 10f;

        public Transform handPosition;

        public float bulletSpeed = 10000f;

        private int activeBullet = 0;
        private List<GameObject> clones = new List<GameObject>();
        private GameObject currentOrbe;
        private bool rotationReset = true;
        private bool joined = false;
        private bool coroutineCalled = false;

        public List<GameObject> GetClones()
        {
            return clones;
        }

        void Start()
        {
            SpawnOrbes();
        }

        // Update is called once per frame
        void Update()
        {
            if(activeBullet <= 0 && !coroutineCalled)
            {
                StartCoroutine(WaitAndSpawn());
                coroutineCalled = true;
            }

            if (PlayerIsInFireMode())
            {
                PrepareBullet();

                if(joined && PlayerHasFired())
                {
                    MoveBullet();
                }
            }

            if (!PlayerIsInFireMode() && !rotationReset)
            {
                foreach(GameObject orbeClone in clones)
                {
                    if (orbeClone != null)
                    {
                        Bullet bullet = orbeClone.GetComponentInChildren<Bullet>();
                        if (!bullet.Shooted())
                        {
                            bullet.DisableElements();
                            orbeClone.GetComponent<OrbeRotation>().resetRotation();
                        }
                    }
                }
                rotationReset = true;
                joined = false;
            } 
        }

        void SpawnOrbes()
        {
            clones.Clear();

            GameObject orbe0 = Instantiate<GameObject>(orbes[activeBullet], parent);
            orbe0.GetComponent<OrbeRotation>().center = center;
            clones.Add(orbe0);
            activeBullet++;

            GameObject orbe1 = Instantiate<GameObject>(orbes[activeBullet], parent);
            orbe1.GetComponent<OrbeRotation>().center = center;
            clones.Add(orbe1);
            activeBullet++;

            coroutineCalled = false;
        }

        void PrepareBullet()
        {
            if (activeBullet > 0)
            {
                currentOrbe = clones[activeBullet - 1];
                currentOrbe.GetComponent<OrbeRotation>().StopRotation();

                Bullet bullet = currentOrbe.GetComponentInChildren<Bullet>();
                if ((bullet.transform.position - handPosition.position).magnitude <= 0.1f) // particle to play here
                {
                    joined = true;
                    bullet.transform.position = handPosition.position;
                    bullet.EnableElements();
                }
                else
                {
                    joined = false;
                    if (!joined)
                    {
                        bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, handPosition.position, 0.8f);
                        bullet.transform.rotation = handPosition.rotation;
                    }
                }
                rotationReset = false;
            }
            /*else
            {
                StartCoroutine(WaitAndSpawn());
            }*/
        }

        IEnumerator WaitAndSpawn()
        {
            yield return new WaitForSeconds(waitTime);
            SpawnOrbes();
            //Debug.Log(activeBullet);
            //Debug.Log(clones.Count);
        }

        void MoveBullet()
        {
            if (currentOrbe != null)
            {
                Bullet bullet = currentOrbe.GetComponentInChildren<Bullet>();
                if (!bullet.Shooted())
                {
                    bullet.SetShooted();
                    //bullet.transform.position += bullet.transform.forward * bulletSpeed * Time.deltaTime;
                    bullet.GetRigidBody().AddForce(bullet.transform.forward * bulletSpeed);
                    bullet.EnableGravity();
                    DestroyCurrentOrbe();
                }
            }
        }

        void DestroyCurrentOrbe()
        {
            Destroy(currentOrbe, 3f);
            activeBullet--;
        }

        bool PlayerIsInFireMode()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("FirePose") ||
                   animator.GetCurrentAnimatorStateInfo(0).IsName("KyleFire");
        }

        bool PlayerHasFired()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("KyleFire");
        }
    }
}