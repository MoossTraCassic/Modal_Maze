using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace ModalFunctions.Utils
{
    public class SimpleInvertCameraAxis : MonoBehaviour
    {
        public List<CinemachineFreeLook> cinemachineFreeLooks;
        public void InvertAxis()
        {
            for(int i=0; i < cinemachineFreeLooks.Count; i++)
            {
                cinemachineFreeLooks[i].m_XAxis.m_InvertAxis = !cinemachineFreeLooks[i].m_XAxis.m_InvertAxis;
                cinemachineFreeLooks[i].m_YAxis.m_InvertAxis = !cinemachineFreeLooks[i].m_YAxis.m_InvertAxis;
            }
        }
    }
}