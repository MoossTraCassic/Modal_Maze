using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ModalFunctions.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class SimpleAudioPlayer : MonoBehaviour
	{
		[Serializable]
        public class SoundBank
        {
            public string name;
            public AudioClip[] clips;
        }

        public bool randomizePitch = true;
        public float pitchRandomRange = 0.2f;
        public float playDelay = 0;
        public SoundBank defaultBank = new SoundBank();

        [HideInInspector]
        public bool playing;
        [HideInInspector]
        public bool canPlay;

        protected AudioSource m_Audiosource;

        public AudioSource audioSource { get { return m_Audiosource; } }

        public AudioClip clip { get; private set; }

        void Awake()
        {
            m_Audiosource = GetComponent<AudioSource>();
        }


		public void PlayRandomClip()
        {
            clip = InternalPlayRandomClip(bankId: 0);
        }

        AudioClip InternalPlayRandomClip(int bankId)
        {
            SoundBank[] banks = null;
            var bank = defaultBank;
            if (bank.clips == null || bank.clips.Length == 0)
                return null;
            var clip = bank.clips[UnityEngine.Random.Range(0, bank.clips.Length)];

            if (clip == null)
                return null;

            m_Audiosource.pitch = randomizePitch ? UnityEngine.Random.Range(1.0f - pitchRandomRange, 1.0f + pitchRandomRange) : 1.0f;
            m_Audiosource.clip = clip;
            m_Audiosource.PlayDelayed(playDelay);

            return clip;
        }
	}
}