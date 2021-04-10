using System;
using System.Collections;
using System.Collections.Generic;
using Door;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial.Level2
{
    public class TutorialManagerLevel2 : MonoBehaviour
    {
        [SerializeField] private LevelComplete overTheDoor;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image backGround;
        [SerializeField] private KeyCollected key;
        [SerializeField] private KeyCollected collected;


        void Update()
        {
            if (frontOfTheDoor())
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

        private bool frontOfTheDoor()
        {
            if (overTheDoor.inDoor && key)
            {
                collected.gameObject.SetActive(true);
                return true;
            }

            collected.gameObject.SetActive(false);
            return false;
        }
    }

}
