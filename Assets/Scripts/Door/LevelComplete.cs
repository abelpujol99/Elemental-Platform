using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Character;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Door
{
    public class LevelComplete : MonoBehaviour
    {

        [SerializeField] private GameObject _transition;
        [SerializeField] private int _neededKeys;
        public bool inDoor;
        private bool _doorUnlock;
        
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.CompareTag("Player"))
            {
                inDoor = true;
            }
        }
    
        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.CompareTag("Player"))
            {
                inDoor = false;
            }
        }
        
    
        private void Start()
        {
            
            if (_neededKeys == 0)
            {
                _doorUnlock = true;
            }
            else
            {
                _doorUnlock = false;
            }

        }
    
        private void Update()
        {
            if (inDoor && Input.GetKey(KeyCode.E) && _doorUnlock)
            {
                _transition.SetActive(true);
                StartCoroutine(ChangeScene());
            }
        }
    
        public void countKeys()
        {
            _neededKeys -= 1;
            if (_neededKeys == 0)
            {
                _doorUnlock = true;
            }
    
        }
    
        public IEnumerator ChangeScene()
        {
            yield return new WaitForSeconds(0.85f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    
    }
}


