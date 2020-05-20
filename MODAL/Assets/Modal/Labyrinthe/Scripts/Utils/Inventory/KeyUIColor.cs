using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ModalFunctions.Utils
{
    public class KeyUIColor : MonoBehaviour
    {
        public static KeyUIColor Instance { get; protected set; }

        public GameObject keyIconPrefab;
        public string[] keyNames;
        
        [SerializeField]
        public List<Material> materialList; 

        private MeshRenderer m_IconRenderer;
        private Color m_OriginalCoreMaterial;
        private Color m_UpdatedColor;
        private Material m_CoreMaterial;

 
        protected const float k_KeyIconAnchorWidth = 0.041f;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SetInitialKeyCount();
        }

        public void SetInitialKeyCount()
        {
 
            GameObject healthIcon = Instantiate(keyIconPrefab);
            healthIcon.transform.SetParent(transform);
            RectTransform healthIconRect = healthIcon.transform as RectTransform;
            healthIconRect.anchoredPosition = Vector2.zero;
            healthIconRect.sizeDelta = Vector2.zero;
            healthIconRect.anchorMin -= new Vector2(k_KeyIconAnchorWidth, 0f);
            healthIconRect.anchorMax -= new Vector2(k_KeyIconAnchorWidth, 0f);
            healthIconRect.anchoredPosition3D = new Vector3(0f, 120f, 0f);
            healthIconRect.localScale = Vector3.one * 80f;
            healthIconRect.localRotation = Quaternion.identity;

            m_CoreMaterial = keyIconPrefab.GetComponent<MeshRenderer>().sharedMaterials[0];
 
            m_OriginalCoreMaterial = m_CoreMaterial.GetColor("_Color");
  
        }

        public void ResetMaterial()
        {
            if(m_OriginalCoreMaterial != null) m_CoreMaterial.SetColor("_Color", m_OriginalCoreMaterial);            
        }

        public void ChangeKeyUI(InventoryController controller)
        {
            
            m_UpdatedColor = m_OriginalCoreMaterial;

            for (int i = 0; i < keyNames.Length; i++)
            {
 
                if (controller.HasItem(keyNames[i]))
                {
                    Debug.Log("ColorUp");
                    m_UpdatedColor = Color.Lerp(m_UpdatedColor, materialList[i].GetColor("_Color"), 0.6f);// += materialDic(keyNames[i]);
                }
            }
 
            if(m_CoreMaterial != null)m_CoreMaterial.SetColor("_Color", m_UpdatedColor);
        }

    }
}