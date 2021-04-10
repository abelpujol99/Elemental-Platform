using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Door
{
    public class KeyCollected : MonoBehaviour
    {

        [SerializeField] private LevelComplete door;
        private bool collected = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !collected)
            {
                collected = true;
                GetComponent<SpriteRenderer>().enabled = false;
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                Destroy(gameObject, 0.5f);
                door.countKeys();
            }
        }
    }
}

