using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ModalFunctions.Utils
{
    public class TimeManager : MonoBehaviour
    {
        public float slowDownFactor = 0.05f;
        public float slowDownLength = 2f;

        private float seconds = 0f;
        private bool slowMotion = false;
        private float oneSecond = 1f;

        private void Update()
        {
            Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
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
            }*/
            
        }

        public void secondsToPast(float secondsToReach)
        {
            StartCoroutine(CountSeconds(secondsToReach));
            //StopCoroutine(CountSeconds(secondsToReach));
        }
        public void DoSlowDown()
        {
            //slowMotion = true;
            Time.timeScale = slowDownFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.05f;
        }

        IEnumerator CountSeconds(float secondsToReach)
        {
            yield return new WaitForSecondsRealtime(secondsToReach);
            seconds++;
        }
    }
}