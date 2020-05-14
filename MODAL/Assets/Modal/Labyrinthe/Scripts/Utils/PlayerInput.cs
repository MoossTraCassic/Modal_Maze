using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Utils
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance
        {
            get { return s_Instance; }
        }

        protected static PlayerInput s_Instance;

        protected bool m_Pause, m_triangle, m_square, m_circle;
        protected bool m_ExternalInputBlocked;

        public bool Pause
        {
            get { return m_Pause; }
        }

        public bool Triangle
        {
            get { return m_triangle; }
        }

        public bool Square
        {
            get { return m_square; }
        }

        public bool Circle
        {
            get { return m_circle; }
        }

        void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if (s_Instance != this)
                throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
        }


        void Update()
        {
            m_Pause = Input.GetButtonDown("Pause");
            m_square = Input.GetButtonDown("Square");
            m_triangle = Input.GetButtonDown("Triangle");
            m_circle = Input.GetButtonDown("Circle");
        }

        public bool HaveControl()
        {
            return !m_ExternalInputBlocked;
        }

        public void ReleaseControl()
        {
            m_ExternalInputBlocked = true;
        }

        public void GainControl()
        {
            m_ExternalInputBlocked = false;
        }
    }
}