using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Door;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial.Level1
{
    public class TutorialManagerLevel1 : MonoBehaviour
{
    [SerializeField] private CharacterScript character;
    [SerializeField] private LevelComplete overTheDoor;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image backGround;
    [SerializeField] private GameObject[] controls;
    private bool jumped;
    private bool E;

    void Start()
    {
        StartCoroutine(WaitUntilTransitionEnd());
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || jumped)
        {
            text.text = "";
            controls[0].SetActive(false);
            controls[1].SetActive(false);
            controls[2].SetActive(false);
            controls[3].SetActive(false);
            backGround.gameObject.SetActive(false);
        }
        
        jumpTutorial();
        
        if (frontOfTheDoor())
        {
            text.text = "Press     to cross\nthe door";
            controls[7].SetActive(true);
            backGround.gameObject.SetActive(true);
            pressedE();
            if (E)
            {
                text.gameObject.SetActive(false);
                backGround.gameObject.SetActive(false);
                controls[4].GetComponent<SpriteRenderer>().enabled = false;
                controls[5].GetComponent<SpriteRenderer>().enabled = false;
                controls[6].GetComponent<SpriteRenderer>().enabled = false;
                controls[7].SetActive(false);
            }
        } 
        else if(character.jumpUpgrade && !jumped)
        {
            text.text = "   Now you can jump\n   Press             ,\n   or ";
            backGround.gameObject.SetActive(true);
            controls[4].SetActive(true);
            controls[5].SetActive(true);
            controls[6].SetActive(true);
        }
    }

    private IEnumerator WaitUntilTransitionEnd()
    {
        yield return new WaitForSeconds(0.85f);
        text.text = "  Move Left: <color=#0073FF>      - </color>\n\n  Move Right:<color=#0073FF>      - </color>";
        controls[0].SetActive(true);
        controls[1].SetActive(true);
        controls[2].SetActive(true);
        controls[3].SetActive(true);
        backGround.gameObject.SetActive(true);
    }
 
    private void jumpTutorial()
    {
        if (character.animator.GetBool("Jump"))
        {
            controls[4].SetActive(false);
            controls[5].SetActive(false);
            controls[6].SetActive(false);
            jumped = true;
        }
    }
    
    private bool frontOfTheDoor()
    {
        if (overTheDoor.inDoor)
        {
            controls[4].SetActive(false);
            controls[5].SetActive(false);
            controls[6].SetActive(false);
            return true;
        }
        controls[7].SetActive(false);
        return false;
    }

    private void pressedE()
    {
        if (Input.GetKey(KeyCode.E))
        {
            E = true;
        }
    }
}

}

