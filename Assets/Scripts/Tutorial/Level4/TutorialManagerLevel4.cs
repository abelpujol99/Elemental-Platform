using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Door;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial.Level4
{
    public class TutorialManagerLevel4 : MonoBehaviour
    {
        [SerializeField] private CharacterScript character;
        [SerializeField] private LevelComplete overTheDoor;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image backGround;
        [SerializeField] private GameObject[] controls;
        private bool doubleJumped;

        void Update()
        {
            if (character.doubleJumpUpgrade && !doubleJumped)
            {
                text.text = "Now you can Double Jump\nTry<color=#0073FF>            +</color>             ,\n<color=#0073FF>      +</color>      or<color=#0073FF>      +</color>";
                controls[0].SetActive(true);
                controls[1].SetActive(true);
                controls[2].SetActive(true);
                controls[3].SetActive(true);
                controls[4].SetActive(true);
                controls[5].SetActive(true);
                backGround.gameObject.SetActive(true);
            }
        
            doubleJumpTutorial();
        
        }

        private void doubleJumpTutorial()
        {
            if (character.animator.GetBool("DoubleJump"))
            {
                text.text = "";
                controls[0].SetActive(false);
                controls[1].SetActive(false);
                controls[2].SetActive(false);
                controls[3].SetActive(false);
                controls[4].SetActive(false);
                controls[5].SetActive(false);
                backGround.gameObject.SetActive(false);
                doubleJumped = true;
            }
        }
    }

}
