using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ModalFunctions.Utils
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get { return s_instance; } }
        protected static TimeManager s_instance;

        public float slowDownFactor = 0.05f;
        public float slowDownLength = 2f;

        private float seconds = 0f;
        private bool slowMotion = false;
        private float oneSecond = 1f;
        private bool timePassed = false;
        private IEnumerator timeCount;

        private float m_initialFixeddeltaTime;
        /*
        void Update()
        {
            if (Time.timeScale != 1f)
            {
                print("restored");
            }
            Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            //}
           /* if (slowMotion)
            {
                //StartCoroutine(CountSeconds(oneSecond));
                //Debug.Log(seconds);
                if (seconds >= slowDownLength)
                {
                    Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
                    Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
                    
                }
                if(Time.timeScale == 1f)
                {
                    //StopCoroutine(CountSeconds(oneSecond));
                    slowMotion = false;
                    Debug.Log("Done");
                }
            }
            
        } 
        */
        private void Awake()
        {
            s_instance = this;
        }

        public void PassTime(float secondsToReach)
        {
            if(timeCount != null)
            {
                StopCoroutine(timeCount);
            }

            timeCount = CountSeconds(secondsToReach);
            StartCoroutine(timeCount);
            //StopCoroutine(CountSeconds(secondsToReach));
        }
        public void DoSlowDown()
        {
            m_initialFixeddeltaTime = Time.fixedDeltaTime;
            Debug.Log(m_initialFixeddeltaTime);
            Time.timeScale = slowDownFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            StartCoroutine(ResetTime());
        }

        IEnumerator ResetTime()
        {
            while(Time.timeScale < 1f)
            {
                Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
                Time.fixedDeltaTime += (1f / slowDownLength) * Time.unscaledDeltaTime;
                Time.fixedDeltaTime = Mathf.Clamp(Time.unscaledDeltaTime, 0f, m_initialFixeddeltaTime);
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
                yield return null;
            }
            Time.timeScale = 1f;
            Time.fixedDeltaTime = m_initialFixeddeltaTime;
            Debug.Log("Slow Mo Done");
            
        }

        public void UnDoSlowMotion()
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        IEnumerator CountSeconds(float secondsToReach)
        {
            seconds = 0f;
            while (seconds < secondsToReach)
            {
                seconds++;
                print(seconds);
                yield return new WaitForSecondsRealtime(oneSecond);
            }
            timePassed = true;
        }

        public bool TimePassed()
        {
            return timePassed;
        }
        public void ResetTimePassed()
        {
            timePassed = false;
            if(timeCount != null)
            {
                print("time Stoped");
                StopCoroutine(timeCount);
            }
        }
    }
}