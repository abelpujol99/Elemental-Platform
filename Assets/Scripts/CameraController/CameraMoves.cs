using System;
using Character;
using UnityEngine;

namespace CameraController
{
    public class CameraMoves : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform offset;

        private float smoothSpeed = 0.125f;

        private void LateUpdate()
        {
            Vector3 desiredPosition = new Vector3(target.position.x + offset.position.x, target.position.y + offset.position.y, offset.position.z);
            transform.position = desiredPosition;
            
            transform.LookAt(target);

        }
    }
}