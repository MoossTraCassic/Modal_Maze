using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ModalFunctions.Controller;

namespace ModalFunctions.Utils
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance
        {
            get { return s_instance; }
        }

        [Tooltip("Could be the Player")]
        public GameObject gameObjectToTeleport;
        public float teleportMinDistance = 20f;

        public Transform[] teleportTransforms;
        public UnityEvent OnBlueActivation, OnGreenActivation, OnRedActivation;

        [SerializeField]
        private Destination m_destination;

        protected static ItemManager s_instance;

        [SerializeField]
        private InventoryController.InventoryChecker m_blueCheker, m_redCheker, m_greenCheker;
        private InventoryController m_inventoryController;

        private bool m_hasPressedTriangle, m_hasPressedSquare, m_hasPressedCircle;

        private void Start()
        {
            s_instance = this;

            m_inventoryController = PlayerController.instance.Inventory;
            if (m_inventoryController == null)
                throw new UnityException("There is no inventoryController attached");
            if(m_destination == null) m_destination = FindObjectOfType<Destination>();
        }

        private void Update()
        {
            if (PlayerInput.Instance != null)
            {
                m_hasPressedTriangle = PlayerInput.Instance.Triangle;
                m_hasPressedSquare = PlayerInput.Instance.Square;
                m_hasPressedCircle = PlayerInput.Instance.Circle;

                if (m_hasPressedTriangle)
                {
                    BlueEffect();
                }
                if (m_hasPressedSquare)
                {
                    GreenEffect();
                }
                if (m_hasPressedCircle)
                {
                    RedEffect();
                }
            }
        }


        void BlueEffect()
        {
            if (m_blueCheker == null) return;

            if (m_blueCheker.CheckInventory(m_inventoryController))
            {
                OnBlueActivation.Invoke();

                ActivateCanGoinAir();
                StartCoroutine(MakeJump());
                for (int i = 0; i < m_blueCheker.inventoryItems.Length; i++)
                    m_inventoryController.RemoveItem(m_blueCheker.inventoryItems[i]);
            }
        }

        void ActivateCanGoinAir()
        {
            PlayerController.instance.canGoInAir = true;
        }

        IEnumerator MakeJump()
        {
            PlayerController.instance.p_jump = true;
            yield return null;
            PlayerController.instance.p_jump = false;
        }

        void GreenEffect()
        {
            if (m_greenCheker == null) return;

            if (m_greenCheker.CheckInventory(m_inventoryController))
            {
                OnGreenActivation.Invoke();
                TeleportToTheNextDestination();
                for (int i = 0; i < m_greenCheker.inventoryItems.Length; i++)
                    m_inventoryController.RemoveItem(m_greenCheker.inventoryItems[i]);
            }
        }

        void TeleportToTheNextDestination()
        {
            Transform teleportPoint = CloserPointToTeleport(teleportTransforms, m_destination);
            GameObjectTeleporter.Teleport(gameObjectToTeleport, teleportPoint);
        }

        Transform CloserPointToTeleport(Transform[] teleportPoints, Destination destination)
        {
            Vector3 toDest = PlayerController.instance.transform.position - destination.transform.position;
            toDest.y = 0;
            float playerDistToDestination = toDest.sqrMagnitude;

            Transform teleportTo = teleportPoints[0];// PlayerController.instance.transform;

            for(int i=0; i<teleportPoints.Length; i++)
            {
                Vector3 pointToDest = teleportPoints[i].position - destination.transform.position;
                pointToDest.y = 0;
                float pointDistToDestination = pointToDest.sqrMagnitude;
                if (playerDistToDestination - pointDistToDestination > teleportMinDistance) teleportTo =  teleportPoints[i];
            }
            return teleportTo;
        }

        void RedEffect()
        {
            OnRedActivation.Invoke();

            if (m_redCheker == null) return;

            if (m_redCheker.CheckInventory(m_inventoryController))
            {
                MakeSlowMotion();
                for (int i = 0; i < m_redCheker.inventoryItems.Length; i++)
                    m_inventoryController.RemoveItem(m_redCheker.inventoryItems[i]);
            }
        }

        void MakeSlowMotion()
        {
            TimeManager.Instance.DoSlowDown();
        }
    }
}