using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Enemy.EnemySpike
{
    public class EnemySpike : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb2D;
        
        [SerializeField] private float _distance, _speed, _startTime, _timeBeforeMove;

        private Vector3 _targetPosition, _lastTargetPosition, _initialPosition;
        
        private float _beforeMoveTimer;

        private void Start()
        {
            _initialPosition = transform.position;
            _beforeMoveTimer = _timeBeforeMove;
            SetTarget();
            _lastTargetPosition = transform.position;
        }

        private void Update()
        {
            if (_startTime > 0)
            {
                _startTime -= Time.deltaTime;
                return;
            }
            
            if (_initialPosition == transform.position && _beforeMoveTimer > 0)
            {
                _beforeMoveTimer -= Time.deltaTime;
                return;
            }

            _beforeMoveTimer = _timeBeforeMove;
            
            CheckPosition();
            
            Move();
        }

        private void SetTarget()
        {
            if (_rb2D.constraints == RigidbodyConstraints2D.FreezePositionX)
            {
                _targetPosition = new Vector3(transform.position.x, transform.position.y + _distance, 0);
            }
            else if (_rb2D.constraints == RigidbodyConstraints2D.FreezePositionY)
            {
                _targetPosition = new Vector3(transform.position.x + _distance, transform.position.y, 0);
            }
        }
        
        private void CheckPosition()
        {
            if (transform.position == _targetPosition)
            {
                _targetPosition = _lastTargetPosition;
                _lastTargetPosition = transform.position;
                _distance *= -1;
                SetTarget();
            }
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<PlayerRespawn>().PlayerDamage();
            }
        }
    }
}

