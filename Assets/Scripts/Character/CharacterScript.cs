using System.Collections;
using System.Collections.Generic;
using CameraController;
using UnityEngine;

namespace Character
{
    public class CharacterScript : MonoBehaviour
    {
        [System.Serializable]
        public class Ability
        {
            private string tag;
            private GameObject ability;
            private int size;
            private float timer;

            public Ability(string tag, GameObject ability, int size, float timer)
            {
                this.tag = tag;
                this.ability = ability;
                this.size = size;
                this.timer = timer;
            }


            public string getTag()
            {
                return tag;
            }

            public GameObject getAbility()
            {
                return ability;
            }

            public int getSize()
            {
                return size;
            }

            public float getTimer()
            {
                return timer;
            }

            public void setSize(int size)
            {
                this.size = size;
            }

            public void setTimer(float timer)
            {
                this.timer = timer;
            }
        }
        
        public enum Abilities {JUMP_UPGRADE, DOUBLE_JUMP_UPGRADE, ROCK, WATER, FIRE, WIND, LIGHTNING}

        public static bool jumpUpgrade;
        public static bool doubleJumpUpgrade;
        public static bool _rockUpgrade;
        public static bool _waterUpgrade;
        public static bool _fireUpgrade;
        public static bool _windUpgrade;
        public static bool _lightningUpgrade;
        
        [SerializeField] private GameObject rock;
        [SerializeField] private GameObject water;
        [SerializeField] private GameObject fire;
        [SerializeField] private GameObject wind;
        [SerializeField] private GameObject lightning;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] public Animator animator;

        private Rigidbody2D _rb2D;
        
        private Vector3 _abilityPos;
        private Vector3 _posCamera;

        private List<Ability> _abilities;
        
        private Dictionary<string, Queue<GameObject>> abilityDictionary;

        private Ability _rocksList;
        private Ability _waterList;
        private Ability _fireList;
        private Ability _windList;
        private Ability _lightningList;
        
        private float _moveSpeed = 2;
        private float _jumpSpeed = 3;
        private float _doubleJumpSpeed = 2.5f;
        private float _fallMultiplier = 0.5f;
        private float _lowJumpMultiplier = 1f;
        private float _maximumZoomIn = 0.54f;
        private float _maximumZoomOut;
        
        private bool _run;
        private bool _jump;
        private bool _fall;
        private bool _doubleJump;
        private bool _canDoubleJump;
        
        private int _powerNum;
        private int _maxPowers;
        private int _rockCapacity = 2;
        private int _waterCapacity = 1;
        private int _fireCapacity = 1;
        private int _windCapacity = 1;
        private int _lightningCapacity = 1;
        private int _speed = 50;


        void Start()
        {
            _maximumZoomOut = Camera.main.orthographicSize;
            _posCamera = Camera.main.transform.position;
            _rb2D = GetComponent<Rigidbody2D>();
            _rockUpgrade = false;
            SetPowers();
            jumpUpgrade = true;
            //doubleJumpUpgrade = true;
        }

        void Update()
        {
            
            Debug.Log(doubleJumpUpgrade);

            Jump();
            
            if (!Input.GetMouseButton(1))
            {
                Zoom();
            }
            else
            {
                _powerNum = ChoosePower(_powerNum, _maxPowers);
                
                if (Input.GetMouseButtonUp(0) && _maxPowers > 0)
                {
                    UsePower(_powerNum);
                }
            }
            
        }

        void FixedUpdate()
        {
            
            LeftRightMove();
            
            LowHighJump();
            
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.CompareTag("Platform"))
            {
                CheckGround.isGrounded = true;
            }
        }

        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.CompareTag("Platform"))
            {
                CheckGround.isGrounded = false;
            }
        }

        private void LeftRightMove()
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                spriteRenderer.flipX = false;
                _rb2D.velocity = new Vector2(_moveSpeed, _rb2D.velocity.y);
                _run = true;
                animator.SetBool("Run", _run);
            } 
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                spriteRenderer.flipX = true;
                _rb2D.velocity = new Vector2(-_moveSpeed, _rb2D.velocity.y);
                _run = true;
                animator.SetBool("Run", _run);
            }
            else
            {
                _rb2D.velocity = new Vector2(0, _rb2D.velocity.y);
                _run = false;
                animator.SetBool("Run", _run);
            }
        }

        private void Jump()
        {
            if (jumpUpgrade)
            {

                if (CheckGround.isGrounded && !_canDoubleJump)
                {
                    _canDoubleJump = true;
                }
                
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                {
                    if (CheckGround.isGrounded)
                    {
                        _rb2D.velocity = new Vector2(_rb2D.velocity.x, _jumpSpeed);
                    }
                    else if (doubleJumpUpgrade && _canDoubleJump && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
                    {
                        if (_rb2D.velocity.y < 0)
                        {
                            _rb2D.velocity = new Vector2(_rb2D.velocity.x, _doubleJumpSpeed);
                            animator.SetBool("DoubleJump", _canDoubleJump);
                            _canDoubleJump = false;
                        }
                        else
                        {
                            _doubleJump = true;
                            animator.SetBool("DoubleJump", _doubleJump);
                            _rb2D.velocity = new Vector2(_rb2D.velocity.x, _doubleJumpSpeed);
                            _doubleJump = false;
                            _canDoubleJump = false;

                        }
                    }
                }
        
                if (_rb2D.velocity.y > 0 && !CheckGround.isGrounded)
                {
                    _run = false;
                    _jump = true;
                    animator.SetBool("Run", false);
                    animator.SetBool("Jump", true);
                }
                else if (_rb2D.velocity.y < 0 && !CheckGround.isGrounded)
                {
                    _jump = false;
                    _run = false;
                    _fall = true;
                    animator.SetBool("Jump", _jump);
                    animator.SetBool("Run", _run);
                    animator.SetBool("Fall", _fall);
                    animator.SetBool("DoubleJump", _doubleJump);
                }
                else if (_jump && CheckGround.isGrounded)
                {
                    _jump = false;
                    animator.SetBool("Jump", _jump);
                }
                else if (_fall && CheckGround.isGrounded)
                {
                    _fall = false;
                    animator.SetBool("Fall", _fall);
                }
                
            }
        }

        private void LowHighJump()
        {
            if (_rb2D.velocity.y < 0)
            {
                _rb2D.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier) * Time.deltaTime;
            }
            else if (_rb2D.velocity.y > 0 && !(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
            {
                _rb2D.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier) * Time.deltaTime;
            }
        }

        private void SetPowers()
        {
            if (_rockUpgrade)
            {
                _abilities = new List<Ability>();
                
                _rocksList = new Ability("Rock", rock, _rockCapacity, 0f);
                _abilities.Add(_rocksList);
                _maxPowers += 1;

                if (_waterUpgrade)
                {
                    _waterList = new Ability("Water", water, _waterCapacity, 1f);
                    _abilities.Add(_waterList);
                    _maxPowers += 1;

                    if (_fireUpgrade)
                    {
                        _fireList = new Ability("Fire", fire, _fireCapacity, 2f);
                        _abilities.Add(_fireList);
                        _maxPowers += 1;

                        if (_windUpgrade)
                        {
                            _windList = new Ability("Wind", wind, _windCapacity, 3f);
                            _abilities.Add(_windList);
                            _maxPowers += 1;

                            if (_lightningUpgrade)
                            {
                                _lightningList = new Ability("Lightning", lightning, _lightningCapacity, 0.5f);
                                _abilities.Add(_lightningList);
                                _maxPowers += 1;
                            }
                        }
                    }
                }
                
                abilityDictionary = new Dictionary<string, Queue<GameObject>>();
            
                foreach (Ability ability in _abilities)
                {
                    Queue<GameObject> abilityPool = new Queue<GameObject>();

                    for (int i = 0; i < ability.getSize(); i++)
                    {
                        GameObject obj = Instantiate(ability.getAbility());
                        obj.SetActive(false);
                        abilityPool.Enqueue(obj);
                    }
                
                    abilityDictionary.Add(ability.getTag(), abilityPool);
                }
            }
        }

        private int ChoosePower(int powerNum, int maxPowers)
        {
            if (maxPowers != 0)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    powerNum += 1;
                    if (powerNum == maxPowers)
                    {
                        powerNum = 0;
                    }
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    powerNum -= 1;
                    if (powerNum == -1)
                    {
                        powerNum = maxPowers - 1;
                    }
                }
            }

            return powerNum;
        }
        
        private void UsePower(int powerNum)
        {
            string tag = "";
            
            GameObject abilityToSpawn;

            switch (powerNum)
            {
                case 0:
                    tag = _abilities[powerNum].getTag();
                    break;
                    
                case 1:
                    tag = _abilities[powerNum].getTag();
                    break;
                    
                case 2:
                    tag = _abilities[powerNum].getTag();
                    break;
                    
                case 3:
                    tag = _abilities[powerNum].getTag();
                    break;
                    
                case 4:
                    tag = _abilities[powerNum].getTag();
                    break;
                        
            }
            
            _abilityPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _abilityPos.z = 0f;
            abilityToSpawn = abilityDictionary[tag].Dequeue();
            abilityToSpawn.SetActive(true);
            abilityToSpawn.transform.position = _abilityPos;
            abilityToSpawn.transform.rotation = Quaternion.identity;
            float abilityYSpeed = abilityToSpawn.GetComponent<Rigidbody2D>().velocity.y; 
            abilityToSpawn.GetComponent<Rigidbody2D>().velocity = new Vector2(0, abilityYSpeed);
            abilityDictionary[tag].Enqueue(abilityToSpawn);
            if (_abilities[powerNum].getTimer() != 0f)
            {
                StartCoroutine(AbilityDisappear(_abilities[powerNum].getTimer(), abilityToSpawn));
            }

        }

        private IEnumerator AbilityDisappear(float timer, GameObject abilityToSpawn)
        {
            yield return new WaitForSeconds(timer);
            abilityToSpawn.SetActive(false);
            
        }

        private void Zoom()
        {
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            if (zoom > 0f)
            {
                if (Camera.main.orthographicSize > _maximumZoomIn)
                {
                    Camera.main.orthographicSize -= 0.1f;
                    Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, _posCamera.z), Time.deltaTime * _speed);
                    //Camera.main.GetComponent<CameraMoves>().enabled = true;
                }
            } 
            else if (zoom < 0f)
            {
                if (Camera.main.orthographicSize < _maximumZoomOut)
                {
                    Camera.main.orthographicSize += 0.1f;
                    Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,  _posCamera, Time.deltaTime * _speed);
                    //Camera.main.GetComponent<CameraMoves>().enabled = false;
                }
            }
        }

        public void ActiveAbility(Abilities ability)
        {
            switch (ability)
            {
                case Abilities.JUMP_UPGRADE:
                    jumpUpgrade = true;
                    break;
                    
                case Abilities.DOUBLE_JUMP_UPGRADE:
                    doubleJumpUpgrade = true;
                    break;
                
                case Abilities.ROCK:
                    _rockUpgrade = true;
                    break;
                
                case Abilities.WATER:
                    _waterUpgrade = true;
                    break;
                
                case Abilities.FIRE:
                    _fireUpgrade = true;
                    break;
                
                case Abilities.WIND:
                    _windUpgrade = true;
                    break;
                
                case Abilities.LIGHTNING:
                    _lightningUpgrade = true;
                    break;
            }
        }
    }
}