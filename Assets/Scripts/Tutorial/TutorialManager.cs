using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private CharacterScript character;
    [SerializeField] private LevelComplete overTheDoor;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image backGround;
    private bool jumped;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitUntilTransitionEnd());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || jumped)
        {
            text.text = "";
            backGround.GetComponent<Image>().enabled = false;
        }
        
        jumpTutorial();
        
        if (frontOfTheDoor())
        {
            text.text = "Press E to croos the door";
            backGround.GetComponent<Image>().enabled = true;
        } 
        else if(CharacterScript.jumpUpgrade && !jumped)
        {
            text.text = "Now you can jump\nCome on, try pressing Space, W or Up Arrow";
            backGround.GetComponent<Image>().enabled = true;
        }
    }

    private IEnumerator WaitUntilTransitionEnd()
    {
        yield return new WaitForSeconds(0.85f);
        text.text = "Move Left: A - Left Arrow\nMove Right: D - Right Arrow";
        backGround.GetComponent<Image>().enabled = true;
    }

    private void jumpTutorial()
    {
        if (character.animator.GetBool("Jump"))
        {
            jumped = true;
        }
    }
    
    private bool frontOfTheDoor()
    {
        if (overTheDoor.inDoor)
        {
            return true;
        }
        return false;
    }
}
