using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{

    [SerializeField] private GameObject transition;
    [SerializeField] private Canvas canvas;
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
        inDoor = false;
    }

    private void Start()
    {
        if (neededKeys == 0)
        {
            doorUnlock = true;
        }
    }

    private void Update()
    {
        Debug.Log(inDoor + "indoor");
        Debug.Log(Input.GetKey(KeyCode.E) + "E");
        Debug.Log(doorUnlock + "unlock");
        if (inDoor && Input.GetKey(KeyCode.E) && doorUnlock)
        {
            canvas.enabled = false;
            transition.SetActive(true);
            StartCoroutine(ChangeScene());
        }
    }

    public void countKeys()
    {
        neededKeys -= 1;
        if (neededKeys == 0)
        {
            Debug.Log("fuck you");
            doorUnlock = true;
        }
        else
        {
            Debug.Log("holiiii");
            doorUnlock = false;
        }

    }

    public IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.85f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
