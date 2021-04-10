using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial.Level3
{
    public class DissapearText1 : MonoBehaviour
    {
    
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image backGround;
        [SerializeField] private PlatformUpDown platform;

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

}
