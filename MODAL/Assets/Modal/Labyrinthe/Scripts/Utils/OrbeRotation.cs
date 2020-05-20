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
        private IEnumerator reset;


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
 

            if (Vector3.Distance(pivot.position , beginPosition) >= translateDistance)
            {
                sens = (beginPosition - pivot.position).y > 0 ? Vector3.up : Vector3.down;
 
            }
            
        }

        public void StopRotation()
        {
            turnSpeed = translateSpeed = 0f;

            if(reset != null) 
            {
                StopCoroutine(reset);
            }
        }

        public void Accelerate()
        {
            turnSpeed = givenTurnSpeed  * (300f / 35f);
            translateSpeed = 0.8f;
        }

        public void Decelerate()
        {
            turnSpeed = givenTurnSpeed;
            translateSpeed = 0.2f;
        }
        public void resetRotation()
        {
            turnSpeed = givenTurnSpeed;
            translateSpeed = 0.2f;
 
            if(reset != null) 
            {
                StopCoroutine(reset);
            }
            reset = resetOrbite();
            StartCoroutine(reset);
        }

        IEnumerator resetOrbite()
        {
            while((bullet.transform.localPosition - orbit).magnitude > 0.05f)
            {
                bullet.transform.localPosition = Vector3.Lerp(bullet.transform.localPosition, orbit, 0.05f);
                yield return null;
            }
            bullet.transform.localPosition = orbit;
 
        }
    }
}