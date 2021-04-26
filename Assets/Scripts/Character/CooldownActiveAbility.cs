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

        private float _actualTime;

        private int _timerNum;

        private List<float> _cooldowns;

        private void Start()
        {
            StartCoroutine(WaitUntilTransitionEnd());
            _cooldowns = new List<float>();
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

        public List<float> UpdateCooldown(List<float> cooldown, int abilityCapacity, float normalAbilityCooldown, float superAbilityCooldown, bool sameAbility)
        {

            _actualTime = Time.timeSinceLevelLoad;
            
            _cooldowns.Clear();

            for (int i = 0; i < abilityCapacity; i++)
            {
                _cooldowns.Add(-((_actualTime - normalAbilityCooldown) - cooldown[i]));
            }

            _cooldowns.Add(-((_actualTime - superAbilityCooldown) - cooldown[cooldown.Count - 1]));
            
            ShowCooldown(_cooldowns, normalAbilityCooldown, superAbilityCooldown, sameAbility);
            
            return _cooldowns;
        }

        public void ShowCooldown(List<float> cooldown, float normalAbilityCooldown, float superAbilityCooldown, bool sameAbility)
        {
            if (gameObject.transform.childCount == 5)
            {
                if (!sameAbility)
                {
                    for (int i = 0; i < gameObject.transform.childCount - 2; i++)
                    {
                        gameObject.transform.GetChild(i + 1).gameObject.SetActive(false);
                    }
                }
                
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                gameObject.transform.GetChild(4).GetComponent<UpdateTimer>().UpdateTimeRemaining(cooldown[cooldown.Count - 1]);
            
                for (int i = 0; i < gameObject.transform.childCount - 2; i++)
                {
                    if (!gameObject.transform.GetChild(i + 1).gameObject.activeSelf)
                    {
                        gameObject.transform.GetChild(i + 1).gameObject.SetActive(true);
                        gameObject.transform.GetChild(i + 1).GetComponent<UpdateTimer>().UpdateTimeRemaining(cooldown[i]);
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