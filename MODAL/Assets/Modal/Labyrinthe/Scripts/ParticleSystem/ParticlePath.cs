using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModalFunctions.Controller;

namespace ModalFunctions.Utils{
	
	public class ParticlePath : MonoBehaviour
	{
        [Tooltip("LIst PArticleSystem to play ")]
    	public ParticleSystem[] pathVFX;
    	
    	private bool m_Play;
    	private bool m_Playing;

    	void Start()
    	{
    		m_Play = false;
				for(int i=0; i<pathVFX.Length; i++)
    					pathVFX[i].gameObject.SetActive(false);
		
    	}

    	void CheckPlayerStatus()
    	{
    		m_Play = PlayerController.instance.p_animator.GetBool("Observe");
    	}

    	void Update()
    	{
            CheckPlayerStatus();

    		if(m_Play && !m_Playing)
    		{
    			Play();
    			m_Playing = true;
    		} 
    		if(!m_Play && m_Playing)
    		{
    			Stop();
    			m_Playing = false;
    		}
    	}

    	void Play()
    	{
    		for(int i=0; i<pathVFX.Length; i++)
		{
			pathVFX[i].gameObject.SetActive(true);
    			pathVFX[i].Play(true);
		}
    	}

    	void Stop()
    	{
    		for(int i=0; i<pathVFX.Length; i++)
    		{
			pathVFX[i].Stop(true);
			pathVFX[i].gameObject.SetActive(false);
		}
    	}

	}
}