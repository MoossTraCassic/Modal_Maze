using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ModalFunctions.Utils
{
    [RequireComponent(typeof(Collider))]
    public class Destination : MonoBehaviour
    {
        public LayerMask layers;
        public UnityEvent OnEnter;
        new Collider collider;

        private bool m_sceneLoading = false;
        /*
        void Reset()
        {
            layers = LayerMask.NameToLayer("Everything");
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }
        */

        private void Start()
        {
            layers = LayerMask.NameToLayer("Everything");
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (0 != (layers.value & 1 << other.gameObject.layer))
            {
                if(!m_sceneLoading)
                {
                    ExecuteOnEnter();
                    m_sceneLoading = true;    
                }
                
            }
        }

        protected virtual void ExecuteOnEnter()
        {
            OnEnter.Invoke();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}