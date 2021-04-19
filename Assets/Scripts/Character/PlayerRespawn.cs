using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Character
{
    public class PlayerRespawn : MonoBehaviour
    {

        private float checkPointPositionX, checkPointPositionY;

        private Animator _animator;

        void Start()
        {
            _animator = GetComponent<Animator>();
            
            if (PlayerPrefs.GetFloat("checkPointPositionX") != 0)
            {
                transform.position = (new Vector2(PlayerPrefs.GetFloat("checkPointPositionX"), PlayerPrefs.GetFloat("checkPointPositionY")));
            }
        }

        public void playerDamage()
        {
            _animator.Play("Hit");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}

