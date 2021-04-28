using System;
using Character;
using UnityEngine;
using UnityEngine.UI;

namespace Ability
{
    public class SuperAbilityCastTime : MonoBehaviour
    {
        private Vector3 _mousePosition;
        
        [SerializeField] private Image _image;

        private float _holdTime;
        private float _time;

        private void Update()
        {
            if (_time < _holdTime)
            {
                _time += Time.deltaTime;
                _image.fillAmount = _time / _holdTime;
            }
            else
            {
                _time = 0;
            }
            
            SetPosition();
        }

        public void HoldedTime(float holdTime)
        {
            _holdTime = holdTime;
        }

        public void ResetFillAmount()
        {
            if (gameObject.activeSelf)
            {
                _image.fillAmount = 0;
                _time = 0;
            }
        }

        private void SetPosition()
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.z = 0;
            transform.position = _mousePosition;
        }
        
    }
}