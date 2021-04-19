using System;
using Character;
using UnityEngine;

namespace CameraController
{
    public class CameraMoves : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform offset;

        private bool _inside;

        private float _smoothSpeed = 3;

        private void Update()
        {
            _smoothSpeed = _smoothSpeed * Time.deltaTime;

            if (!_inside)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), _smoothSpeed * Time.deltaTime);
            }
            
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.CompareTag("Player"))
            {
                _inside = true;
            }
        }

        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.CompareTag("Player"))
            {
                _inside = false;
            }
        }
    }
}