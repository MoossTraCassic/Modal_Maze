using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Utils
{
    public class OrbeRotation : MonoBehaviour
    {
        [Tooltip("Speed of the Rotation and Translation")]
        public float turnSpeed = 25f;
        public float translateSpeed = 0.15f;

        private Transform pivot;
        private Vector3 beginPosition;
        private Vector3 sens;


        private void Start()
        {
            pivot = GetComponent<Transform>();
            beginPosition = new Vector3(pivot.localPosition.x, pivot.localPosition.y, pivot.localPosition.z);
            sens = Vector3.down;
        }
        // Update is called once per frame
        void Update()
        {
            pivot.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

            pivot.Translate(sens * translateSpeed * Time.deltaTime);

            if (Vector3.Distance(pivot.position, beginPosition) >= 2f)
            {
                sens = -sens;
            }
        }
    }
}