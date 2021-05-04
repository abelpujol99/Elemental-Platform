using System;
using UnityEngine;

namespace GameController
{
    public class GameController : MonoBehaviour
    {
        private static GameController _instance;

        public static GameController Instance => _instance;

        [SerializeField] public Camera _camera;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }
    }
}