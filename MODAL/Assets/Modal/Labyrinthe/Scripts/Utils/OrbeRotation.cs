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
        private Vector3 beginPosition;
        private Vector3 sens;
        private float translateDistance = 2f;


        private void Start()
        {
            pivot = GetComponent<Transform>();
            beginPosition = center.position;
            sens = Vector3.down;
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
    }
}