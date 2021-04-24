using System;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class UpdateTimer : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        private float _timeRemaining;
        private float _time;

        private void Update()
        {
            if (_time < _timeRemaining)
            {
                _time += Time.deltaTime;
                _image.fillAmount = _time / _timeRemaining;
            }
            else
            {
                _time = 0;
                gameObject.SetActive(false);
            }
        }

        public void UpdateTimeRemaining(float cooldown)
        {
            _image.fillAmount = 0;
            _timeRemaining = cooldown;
        }
    }
}