using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Utils
{
    public class OrbeRotation : MonoBehaviour
    {
        [Tooltip("Speed of the Rotation and Translation")]
        public float turnSpeed = 35f;
        public float translateSpeed = 0.15f;
        public Transform center;

        private Transform pivot;
        private Vector3 orbit = new Vector3(3.65f , 0.3f, 0f);
        private Vector3 beginPosition;
        private Vector3 sens;
        private Bullet bullet;
        private float translateDistance = 2f;
        private float givenTurnSpeed;


        private void Start()
        {
            pivot = GetComponent<Transform>();
            beginPosition = center.position;
            bullet = GetComponentInChildren<Bullet>();

            sens = Vector3.down;
            givenTurnSpeed = turnSpeed;
        }
        // Update is called once per frame
        void Update()
        {
            beginPosition = center.position;
            pivot.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

            pivot.Translate(sens * translateSpeed * Time.deltaTime);
            //pivot.position += sens * translateSpeed * Time.deltaTime; 

            if (Vector3.Distance(pivot.position , beginPosition) >= translateDistance)
            {
                sens = (beginPosition - pivot.position).y > 0 ? Vector3.up : Vector3.down;
                //sens *= -1;
            }
            
        }

        public void StopRotation()
        {
            turnSpeed = translateSpeed = 0f;
        }

        public void resetRotation()
        {
            turnSpeed = givenTurnSpeed;
            translateSpeed = 0.2f;
            /*
            int interpolation = 50;
            for (int i = 0; i < interpolation; i++)
            {
                bullet.transform.localPosition = Vector3.Lerp(bullet.transform.localPosition, orbit, 0.5f);
            }
            */
            bullet.transform.localPosition = orbit;
        }

        IEnumerator resetOrbite()
        {
            while((bullet.transform.localPosition - orbit).magnitude > 0.05f)
            {
                bullet.transform.localPosition = Vector3.Lerp(bullet.transform.localPosition, orbit, 0.1f);
                yield return null;
            }
            print("Coroutine ended");
        }
    }
}