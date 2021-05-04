using System;
using System.Collections;
using Character;
using UnityEngine;
using Random = System.Random;

namespace CameraController
{
    public class CameraMoves : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private Vector3 offset;

        private float smoothSpeed = 0.125f;
        private Random rnd;

        private void FixedUpdate()
        {
            FixCamera();
            rnd = new Random();
        }

        private void FixCamera()
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

        public IEnumerator Shake(float duration)
        {
            Vector3 originalPos = transform.localPosition;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                int minimum = -1;
                int maxmum = 1;
                
                float x = (float) rnd.NextDouble() * (maxmum - minimum) + minimum;
                float y = (float) rnd.NextDouble() * (maxmum - minimum) + minimum;

                transform.localPosition = new Vector3(x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = originalPos;

        }
    }
}