using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace HeartOfWinter.Arena
{

    public class Timer : MonoBehaviour
    {
        [SerializeField] Text display;

        [SerializeField] float startTime = 20f;
        [SerializeField] float currentTime;

        private void Start()
        {
            StartTimer();
            enabled = false;
        }

        public void StartTimer()
        {
            currentTime = startTime;
            enabled = true;
            if (display != null) display.text = Math.Round(currentTime, 0).ToString();
        }

        public bool done
        {
            get { return currentTime <= 0; }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            currentTime -= Time.deltaTime;

            if (done)
            {
                currentTime = 0;
                enabled = false;
            }

            if (display != null) display.text = Math.Round(currentTime, 0).ToString();
        }
    }

}
