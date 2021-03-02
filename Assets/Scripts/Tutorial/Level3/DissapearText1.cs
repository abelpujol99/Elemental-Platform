using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DissapearText1 : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image backGround;
    [SerializeField] private Platform platform;

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("Player"))
        {
            text.text = "";
            platform.gameObject.SetActive(false);
            backGround.gameObject.SetActive(false);
        }
    }
}
