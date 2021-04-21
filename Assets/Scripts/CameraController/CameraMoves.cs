using System;
using Character;
using UnityEngine;

namespace CameraController
{
    public class CameraMoves : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private float smoothSpeed = 0.125f;
        [SerializeField] private Vector3 offset;
        private void FixedUpdate()
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}