using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scenario
{
    public class HideScenario : MonoBehaviour
    {

        private Color _tilemapColor;

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (!trigger.transform.CompareTag("Player"))
            {
                return;
            }
            gameObject.SetActive(false);
        }
    }
}