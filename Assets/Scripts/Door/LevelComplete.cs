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
    public bool inDoor;
    
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

    private void Update()
    {
        if (inDoor && Input.GetKey(KeyCode.E) && takeKeys())
        {
            Destroy(canvas);
            transition.SetActive(true);
            StartCoroutine(ChangeScene());
        }
    }

    private bool takeKeys()
    {
        if (transform.childCount == 9)
        {
            return true;
        }

        return false;
    }

    public IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.85f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
