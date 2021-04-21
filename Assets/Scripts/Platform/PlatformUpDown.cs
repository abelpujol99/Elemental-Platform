using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platform
{
    public class PlatformUpDown : MonoBehaviour
    {
        private PlatformEffector2D _effector;

        private float _startWaitTime = 0.1f;
        private float _waitedTime;

        private void Start()
        {
            _effector = GetComponent<PlatformEffector2D>();
        }

        private void Update()
        {

            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            {
                _waitedTime = _startWaitTime;
            }
        
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (_waitedTime <= 0)
                {
                    _effector.rotationalOffset = 180;
                    _waitedTime = _startWaitTime;
                }
                else
                {
                    _waitedTime -= Time.deltaTime;
                }
            }

            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                _effector.rotationalOffset = 0;
            }
        }
    }
}

