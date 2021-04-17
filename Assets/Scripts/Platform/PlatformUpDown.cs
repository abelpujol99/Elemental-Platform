using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platform
{
    public class PlatformUpDown : MonoBehaviour
    {
        private PlatformEffector2D effector;

        private float startWaitTime = 0.5f;
        private float waitedTime;

        private void Start()
        {
            effector = GetComponent<PlatformEffector2D>();
        }

        private void Update()
        {

            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            {
                waitedTime = startWaitTime;
            }
        
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (waitedTime <= 0)
                {
                    effector.rotationalOffset = 180;
                    waitedTime = startWaitTime;
                }
                else
                {
                    waitedTime -= Time.deltaTime;
                }
            }

            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                effector.rotationalOffset = 0;
            }
        }
    }
}

