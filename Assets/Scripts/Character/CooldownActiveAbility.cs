using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Character
{
    public class CooldownActiveAbility : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _TMP;

        private int _timerNum;

        private void Start()
        {
            StartCoroutine(WaitUntilTransitionEnd());
        }

        public void ShowAbilities(int powerNum, int size)
        {
            _TMP.SetText("");
            for (int i = 0; i < size; i++)
            {
                _TMP.text += "<sprite index=[" + powerNum + "]>  ";
            }

            if (powerNum != 1)
            {
                _TMP.text += "\n \n<sprite index=[" + (powerNum + 4) + "]>";
            }

        }

        public void UpdateCooldown(List<DateTime> cooldown, bool holded, float normalAbilityCooldown, float superAbilityCooldown)
        {
            Debug.Log(cooldown[cooldown.Count - 1].Second);
            
            if (holded)
            {
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                gameObject.transform.GetChild(4).GetComponent<UpdateTimer>().UpdateTimeRemaining(cooldown[cooldown.Count - 1].Second + superAbilityCooldown);
            }
            else
            {
                for (int i = 0; i < gameObject.transform.childCount - 2; i++)
                {
                    if (!gameObject.transform.GetChild(i + 1).gameObject.activeSelf)
                    {
                        gameObject.transform.GetChild(i + 1).gameObject.SetActive(true);
                        gameObject.transform.GetChild(i + 1).GetComponent<UpdateTimer>().UpdateTimeRemaining(cooldown[i].Second + normalAbilityCooldown);
                        return;
                    }
                }
            }
        }

        private IEnumerator WaitUntilTransitionEnd()
        {
            yield return new WaitForSeconds(0.85f);
            _TMP.enabled = true;
        }
    }
}