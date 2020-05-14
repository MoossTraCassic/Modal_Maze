using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ModalFunctions.Controller;

namespace ModalFunctions.Utils
{
	[RequireComponent(typeof(Collider))]
    public class InteractOnTriggerEnter : MonoBehaviour
    {
        public LayerMask layers;
        //public UnityEvent OnEnter, OnExit;
        new Collider collider;
        //**public InventoryController.InventoryChecker[] inventoryChecks;

        void Reset()
        {
            layers = LayerMask.NameToLayer("Everything");
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (0 != (layers.value & 1 << other.gameObject.layer))
            {
                ExecuteOnEnter(other);
            }
        }

        protected virtual void ExecuteOnEnter(Collider other)
        {
            //OnEnter.Invoke();
            /*for (var i = 0; i < inventoryChecks.Length; i++)
            {
                inventoryChecks[i].CheckInventory(other.GetComponentInChildren<InventoryController>());
            }*/
        }

        void OnTriggerExit(Collider other)
        {
            if (0 != (layers.value & 1 << other.gameObject.layer))
            {
                ExecuteOnExit(other);
            }
        }

        protected virtual void ExecuteOnExit(Collider other)
        {
            //OnExit.Invoke();
            PlayerController.instance.canGoInAir = true;

            Destroy(this.gameObject, 2f);
        }

/*
        void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "InteractionTrigger", false);
        }

        void OnDrawGizmosSelected()
        {
            //need to inspect events and draw arrows to relevant gameObjects.
        }
*/
    } 
}