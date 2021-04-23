using System.Collections;
using Character;
using UnityEngine;

namespace Enemy.Bat
{
    public class Bat : MonoBehaviour
    {
        [SerializeField] private float _health;
        [SerializeField] private float _speed;
        [SerializeField] private float _distance;
        [SerializeField] private float _timeFlying;
        [SerializeField] private float _maxMinHeight;
        
        private bool _onAir;
        private bool _stand;

        private Animator _animator;
        
        private SpriteRenderer _spriteRenderer;
        
        private Rigidbody2D _rb2D;

        private Vector3 _initialPosition;
        private Vector3 _targetPosition;

        private float _timeBeforeFlying = 1f;
        private float _auxTimeBeforeFlying;
        private float _auxTimeFlying;
        private float _height;
        private float _width;
        private float _initialSpeed;
        private float _knockUp;
        private float _slow;
        private float _auxSlow;
        private float _goUp;
        private bool _canMove;
        private bool _knockedUp;
        private bool _top;
        private bool _bottom;

        private string _animationName;

        private void Start()
        {
            _initialPosition = transform.position;
            _auxTimeBeforeFlying = _timeBeforeFlying;
            _auxTimeFlying = _timeFlying;
            _top = true;
            _slow = 5;
            _auxSlow = _slow;
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb2D = GetComponent<Rigidbody2D>();
            _initialSpeed = _speed;
            _knockUp = 3;
            _canMove = true;
            _goUp = 0.1f;
            //Move();
        }
        private void Update()
        {
            if (_onAir)
            {
                _height = Mathf.Sin(Time.time) * _maxMinHeight;
            }

            if (!_onAir)
            {
                _timeBeforeFlying -= Time.deltaTime;
            }
            else
            {
                _timeFlying -= Time.deltaTime;
            }

            if (_timeBeforeFlying <= 0 && !_onAir)
            {
                CeilOut();
                _timeBeforeFlying = _auxTimeBeforeFlying;
            }
            
            if (_health <= 0)
            {
                _animator.Play("Die");
                GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(DestroyBat());
            }


            if (_timeFlying > 0f)
            {
                
                if (Vector3.Distance(transform.position, _initialPosition) >= Vector3.Distance(_initialPosition, _targetPosition))
                {
                    _targetPosition = new Vector3(transform.position.x + _distance, transform.position.y, 0);
                    _initialPosition = transform.position;
                }
                else if (_onAir)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
                }
            }
            else
            {
                _targetPosition = new Vector3(transform.position.x, transform.position.y + _goUp, 0f);
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
            }
        }

        private void CeilOut()
        {
            lookDirection();
            _goUp = 0.1f;
            _animator.SetBool("Fly", true);
            StartCoroutine(StartToMove());
        }
        
        private void Fly()
        {
            lookDirection();
        }
        
        private void CeilIn()
        {
            lookDirection();
            _distance *= -1;
            _animator.SetBool("Fly", false);
        }

        private void lookDirection()
        {
            if (_distance >= 0)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }

        private void Hit()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Fly"))
            {
                _animationName = "Fly";
            }
            
            
            _animator.SetBool("Hit", true);
            _animator.SetBool(_animationName, false);
            StartCoroutine(SetMove("Hit", 0.25f));
        }

        private IEnumerator StartToMove()
        {
            yield return new WaitForSeconds(0.5f);
            _onAir = true;
            _stand = false;
            _targetPosition = new Vector3(transform.position.x, transform.position.y - 0.2f, 0);
            _initialPosition = transform.position;
            Fly();
        }
        
        private IEnumerator ReturnSpeed(float slow)
        {
            yield return new WaitForSeconds(2);
            _speed += _initialSpeed / slow;
        }
        
        private IEnumerator SetMove(string state, float time)
        {
            yield return new WaitForSeconds(time);
            _animator.SetBool(state, false);
            _animator.SetBool(_animationName,  true);
            _canMove = true;
        }
        
        private IEnumerator DestroyBat()
        {
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<PlayerRespawn>().PlayerDamage();
            }
            if (collision.gameObject.CompareTag("Fire"))
            {
                _health -= 2;
                Hit();
            }
            else if (collision.gameObject.CompareTag("Lightning"))
            {
                _health -= 1;
                Hit();
            }
            else if (collision.gameObject.CompareTag("Water"))
            {
                _health -= 0.1f;
                Hit();
            }
            else if (collision.gameObject.CompareTag("Wind"))
            {
                if (_speed == _initialSpeed)
                {
                    StartCoroutine(ReturnSpeed(_slow));
                    _speed -= _speed / _slow;
                }
            }
            else if (collision.gameObject.CompareTag("Tilemap2"))
            {
                if (_onAir)
                {
                    CeilIn();
                    _onAir = false;
                    _stand = true;
                    _goUp = 0;
                    _targetPosition = transform.position;
                    _timeFlying = _auxTimeFlying;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("SuperFire"))
            {
                _health -= 6;
                Hit();
            }
            else if (collision.gameObject.CompareTag("SuperLightning"))
            {
                _health -= 2.5f;
                Hit();
            }
            else if (collision.gameObject.CompareTag("SuperWater"))
            {
                _health -= 0.5f;
                Hit();
            }
            else if (collision.gameObject.CompareTag("SuperWind"))
            {
                if (_speed == _initialSpeed || _speed == _initialSpeed / _slow)
                {
                    _slow *= 1.5f; 
                    StartCoroutine(ReturnSpeed(_slow));
                    _speed -= _speed / _slow;
                    _slow = _auxSlow;
                }
            }
        }
    }
}