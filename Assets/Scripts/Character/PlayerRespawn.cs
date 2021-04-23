using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Character
{
    public class PlayerRespawn : MonoBehaviour
    {

        [SerializeField] private GameObject _transition;
        
        private float checkPointPositionX, checkPointPositionY;

        private Animator _animator;

        private bool _nextLevel;

        void Start()
        {
            _animator = GetComponent<Animator>();
            
            if (PlayerPrefs.GetFloat("checkPointPositionX") != 0)
            {
                transform.position = (new Vector2(PlayerPrefs.GetFloat("checkPointPositionX"), PlayerPrefs.GetFloat("checkPointPositionY")));
            }
        }

        public void PlayerDamage()
        {
            if (!_transition.activeSelf)
            {
                _animator.Play("Hit");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }

}

