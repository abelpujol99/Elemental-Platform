using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial.Level3
{
    public class AppearText1 : MonoBehaviour
    {
    
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image backGround;
        [SerializeField] private PlatformUpDown platform;
        private bool enter;

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.CompareTag("Player") && !enter)
            {
                text.text = "Try to <color=#0073FF>JUMP</color> through";
                platform.gameObject.SetActive(true);
                backGround.gameObject.SetActive(true);
                enter = true;
            }
        }
    }

}
