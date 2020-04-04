using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Utils
{
    public class VirtualCameraTranslation : MonoBehaviour
    {
        private float m_vertical = 0f;
        private Transform topRig;
        private Transform middleRig;
        private Transform bottomRig;
        private Vector3 destination;

        void Start()
        {
            /*
            topRig = gameObject.transform.GetChild(1).transform;
            middleRig = gameObject.transform.GetChild(2).transform;
            bottomRig = gameObject.transform.GetChild(3).transform;
            */    
    }

        void Update()
        {
            m_vertical = Input.GetAxis("RightJoystickVertical");
            MoveVertical();
        }

        void MoveVertical()
        {
            /*
            if(m_vertical > 0)
            {
                destination = m_vertical * bottomRig.position + (1f - m_vertical) * middleRig.position;
            }
            else
            {
                destination = -m_vertical * topRig.position + (1f + m_vertical) * middleRig.position;
            }

            transform.position = Vector3.Lerp(transform.position, destination, 0.3f);
            */
        }
    }
}