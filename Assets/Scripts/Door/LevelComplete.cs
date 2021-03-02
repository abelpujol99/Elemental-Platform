using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{

    [SerializeField] private GameObject transition;
    [SerializeField] private int neededKeys;
    [SerializeField] public bool inDoor;
    private bool doorUnlock;
    
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
        if (neededKeys == 0)
        {
            doorUnlock = true;
        }
        else
        {
            doorUnlock = false;
        }
    }

    private void Update()
    {
        
        if (inDoor && Input.GetKey(KeyCode.E) && doorUnlock)
        {
            transition.SetActive(true);
            StartCoroutine(ChangeScene());
        }
    }

    public void countKeys()
    {
        neededKeys -= 1;
        if (neededKeys == 0)
        {
            doorUnlock = true;
        }

    }

    public IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.85f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
