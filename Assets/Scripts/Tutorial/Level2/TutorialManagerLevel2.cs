using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManagerLevel2 : MonoBehaviour
{
    [SerializeField] private LevelComplete overTheDoor;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image backGround;
    [SerializeField] private KeyCollected key;
    private bool collected;
    
    void Update()
    {
        if (frontOfTheDoor() && !collected)
        {
            text.text = "Take all     to open the door";
            backGround.gameObject.SetActive(true);
        }
        else
        {
            text.text = "";
            backGround.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("Player"))
        {
            collected = true;
        }
    }

    private bool frontOfTheDoor()
    {
        if (overTheDoor.inDoor && !collected)
        {
            key.gameObject.SetActive(true);
            return true;
        }

        key.gameObject.SetActive(false);
        return false;
    }
}
