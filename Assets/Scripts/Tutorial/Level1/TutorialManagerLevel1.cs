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
    [SerializeField] private CharacterScript _character;
    [SerializeField] private LevelComplete _overTheDoor;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _backGround;
    [SerializeField] private GameObject[] _controls;
    private bool _jumped;
    private bool _pressedE;

    void Start()
    {
        StartCoroutine(WaitUntilTransitionEnd());
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || _jumped)
        {
            _text.text = "";
            _controls[0].SetActive(false);
            _controls[1].SetActive(false);
            _controls[2].SetActive(false);
            _controls[3].SetActive(false);
            _backGround.gameObject.SetActive(false);
        }
        
        jumpTutorial();
        
        if (frontOfTheDoor())
        {
            _text.text = "Press     to cross\nthe door";
            _controls[7].SetActive(true);
            _backGround.gameObject.SetActive(true);
            pressedE();
            if (_pressedE)
            {
                _text.gameObject.SetActive(false);
                _backGround.gameObject.SetActive(false);
                _controls[4].GetComponent<SpriteRenderer>().enabled = false;
                _controls[5].GetComponent<SpriteRenderer>().enabled = false;
                _controls[6].GetComponent<SpriteRenderer>().enabled = false;
                _controls[7].SetActive(false);
            }
        } 
        else if(CharacterScript.jumpUpgrade && !_jumped)
        {
            _text.text = "   Now you can jump\n   Press             ,\n   or ";
            _backGround.gameObject.SetActive(true);
            _controls[4].SetActive(true);
            _controls[5].SetActive(true);
            _controls[6].SetActive(true);
        }
    }

    private IEnumerator WaitUntilTransitionEnd()
    {
        yield return new WaitForSeconds(0.85f);
        _text.text = "  Move Left: <color=#0073FF>      - </color>\n\n  Move Right:<color=#0073FF>      - </color>";
        _controls[0].SetActive(true);
        _controls[1].SetActive(true);
        _controls[2].SetActive(true);
        _controls[3].SetActive(true);
        _backGround.gameObject.SetActive(true);
    }
 
    private void jumpTutorial()
    {
        if (_character.animator.GetBool("Jump"))
        {
            _controls[4].SetActive(false);
            _controls[5].SetActive(false);
            _controls[6].SetActive(false);
            _jumped = true;
        }
    }
    
    private bool frontOfTheDoor()
    {
        if (_overTheDoor.inDoor)
        {
            _controls[4].SetActive(false);
            _controls[5].SetActive(false);
            _controls[6].SetActive(false);
            return true;
        }
        _controls[7].SetActive(false);
        return false;
    }

    private void pressedE()
    {
        if (Input.GetKey(KeyCode.E))
        {
            _pressedE = true;
        }
    }
}

}

