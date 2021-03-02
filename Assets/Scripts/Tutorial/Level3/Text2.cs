using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Text2 : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image backGround;
    [SerializeField] private GameObject[] controls;
    private bool enter;

    private void Start()
    {
        controls = gameObject.GetComponents<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("Player") && !enter)
        {
            text.text = "Try to press <color=#0073FF>   -    </color> to go down";
            backGround.gameObject.SetActive(true);
            controls[0].SetActive(true);
            controls[1].SetActive(true);
            enter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.CompareTag("Player"))
        {
            text.text = "";
            backGround.gameObject.SetActive(false);
            controls[0].SetActive(false);
            controls[1].SetActive(false);
        }
    }
    
}
