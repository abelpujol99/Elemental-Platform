using System;
using System.Collections;
using System.Collections.Generic;
using Ability.Abilities;
using System.Reflection;
using System.Security.Cryptography;
using CameraController;
using Ability;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class CharacterScript : MonoBehaviour
    {
        public enum Abilities {JUMP_UPGRADE, DOUBLE_JUMP_UPGRADE, ROCK, WATER, FIRE, WIND, LIGHTNING}

        public static bool isGround;
        public static bool jumpUpgrade;
        public static bool doubleJumpUpgrade;
        public static bool rockUpgrade;
        public static bool waterUpgrade;
        public static bool fireUpgrade;
        public static bool windUpgrade;
        public static bool lightningUpgrade;
        public static bool superRockUpgrade;
        public static bool superWaterUpgrade;
        public static bool superFireUpgrade;
        public static bool superWindUpgrade;
        public static bool superLightningUpgrade;

        [SerializeField] private GameObject _rock;
        [SerializeField] private GameObject _water;
        [SerializeField] private GameObject _fire;
        [SerializeField] private GameObject _wind;
        [SerializeField] private GameObject _lightning;
        [SerializeField] private GameObject _superRock;
        [SerializeField] private GameObject _superWater;
        [SerializeField] private GameObject _superFire;
        [SerializeField] private GameObject _superWind;
        [SerializeField] private GameObject _superLightning;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public Animator animator;

        private Rigidbody2D _rb2D;
        
        private Vector3 _abilityPos;
        private Vector3 _posCamera;

        private List<List<Ability.Ability>> _abilities;
        
        private Dictionary<string, Queue<GameObject>> abilityDictionary;

        private List<Ability.Ability> _rocksList;
        private List<Ability.Ability> _waterList;
        private List<Ability.Ability> _fireList;
        private List<Ability.Ability> _windList;
        private List<Ability.Ability> _lightningList;
        private List<Ability.Ability> _superRocksList;
        private List<Ability.Ability> _superWaterList;
        private List<Ability.Ability> _superFireList;
        private List<Ability.Ability> _superWindList;
        private List<Ability.Ability> _superLightningList;
        
        private float _moveSpeed = 2;
        private float _jumpSpeed = 3;
        private float _doubleJumpSpeed = 2.5f;
        private float _fallMultiplier = 0.5f;
        private float _lowJumpMultiplier = 1f;
        private float _maximumZoomIn = 0.54f;
        private float _maximumZoomOut;
        private float _abilityTime;
        private float _holdTime = 0.75f;
        private float _superAbilityTimer = 0.3f;
        private float _abilityRange;
        private float _maxAbilityRange = 1;
        
        private bool _run;
        private bool _jump;
        private bool _fall;
        private bool _doubleJump;
        private bool _canDoubleJump;
        private bool _useSuperAbility;

        private int _powerNum;
        private int _maxPowers;
        private int _rockCapacity = 2;
        private int _waterCapacity = 1;
        private int _fireCapacity = 1;
        private int _windCapacity = 1;
        private int _lightningCapacity = 1;
        private int _superAbilityCapacity = 1;
        private int _speed = 100;

        void Start()
        {
            _maximumZoomOut = Camera.main.orthographicSize;
            _posCamera = Camera.main.transform.position;
            _rb2D = GetComponent<Rigidbody2D>();
            rockUpgrade = true;
            waterUpgrade = true;
            fireUpgrade = true;
            windUpgrade = true;
            lightningUpgrade = true;
            SetPowers();
            jumpUpgrade = true;
            //doubleJumpUpgrade = true;
            _useSuperAbility = true;
        }

        void Update()
        {

            Debug.DrawLine(transform.position, transform.position + (_abilityPos - transform.position).normalized * _abilityRange, Color.green);
            
            Jump();
            
            if (!Input.GetMouseButton(1))
            {
                Zoom();
            }
            else
            {
                ChoosePower();

                UseNormalOrSuperAbility();
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
                isGround = true;
            }
        }
        
        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.CompareTag("Platform"))
            {
                isGround = false;
            }
        }
        
        private void LeftRightMove()
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                _spriteRenderer.flipX = false;
                _rb2D.velocity = new Vector2(_moveSpeed, _rb2D.velocity.y);
                _run = true;
                animator.SetBool("Run", _run);
            } 
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _spriteRenderer.flipX = true;
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

                if (isGround && !_canDoubleJump)
                {
                    _canDoubleJump = true;
                }
                
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                {
                    if (isGround)
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
        
                if (_rb2D.velocity.y > 0 && !isGround)
                {
                    _run = false;
                    _jump = true;
                    animator.SetBool("Run", false);
                    animator.SetBool("Jump", true);
                }
                else if (_rb2D.velocity.y < 0 && !isGround)
                {
                    _jump = false;
                    _run = false;
                    _fall = true;
                    animator.SetBool("Jump", _jump);
                    animator.SetBool("Run", _run);
                    animator.SetBool("Fall", _fall);
                    animator.SetBool("DoubleJump", _doubleJump);
                }
                else if (_jump && isGround)
                {
                    _jump = false;
                    animator.SetBool("Jump", _jump);
                }
                else if (_fall && isGround)
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
            if (rockUpgrade)
            {
                _abilities = new List<List<Ability.Ability>>();

                _rocksList = new List<Ability.Ability>();

                Rock rockAux = _rock.GetComponent<Rock>();
                rockAux.setTag("Rock");
                rockAux.setAbility(_rock);
                rockAux.setSize(2);
                rockAux.setTimer(2f);
                rockAux.setCast(true);
                rockAux.setCoolDown(2f);

                SuperRock superRockAux = _superRock.GetComponent<SuperRock>();
                superRockAux.setTag("SuperRock");
                superRockAux.setAbility(_superRock);
                superRockAux.setSize(_superAbilityCapacity);
                superRockAux.setTimer(_superAbilityTimer);
                superRockAux.setCast(true);
                superRockAux.setCoolDown(5f);
                
                _rocksList.Add(rockAux);
                _rocksList.Add(superRockAux);
                
                _abilities.Add(_rocksList);
                _maxPowers += 1;

                if (waterUpgrade)
                {
                    _waterList = new List<Ability.Ability>();
                    
                    Water waterAux = _water.GetComponent<Water>();
                    waterAux.setTag("Water");
                    waterAux.setAbility(_water);
                    waterAux.setSize(_waterCapacity);
                    waterAux.setTimer(1f);
                    waterAux.setCast(true);
                    waterAux.setCoolDown(2f);

                    SuperWater superWaterAux = _superWater.GetComponent<SuperWater>();
                    superWaterAux.setTag("SuperWater");
                    superWaterAux.setAbility(_superWater);
                    superWaterAux.setSize(_superAbilityCapacity);
                    superWaterAux.setTimer(_superAbilityTimer);
                    superWaterAux.setCast(true);
                    superWaterAux.setCoolDown(4f);
                    
                    _waterList.Add(waterAux);
                    _waterList.Add(superWaterAux);
                    
                    _abilities.Add(_waterList);
                    _maxPowers += 1;

                    if (fireUpgrade)
                    {
                        _fireList = new List<Ability.Ability>();

                        Fire fireAux = _fire.GetComponent<Fire>();
                        fireAux.setTag("Fire");
                        fireAux.setAbility(_fire);
                        fireAux.setSize(_fireCapacity);
                        fireAux.setTimer(2f);
                        fireAux.setCast(true);
                        fireAux.setCoolDown(3f);

                        SuperFire superFireAux = _superFire.GetComponent<SuperFire>();
                        superFireAux.setTag("SuperFire");
                        superFireAux.setAbility(_superFire);
                        superFireAux.setSize(_superAbilityCapacity);
                        superFireAux.setTimer(_superAbilityTimer);
                        superFireAux.setCast(true);
                        superFireAux.setCoolDown(5f);
                        
                        _fireList.Add(fireAux);
                        _fireList.Add(superFireAux);
                        
                        _abilities.Add(_fireList);
                        _maxPowers += 1;

                        if (windUpgrade)
                        {

                            _windList = new List<Ability.Ability>();

                            Wind windAux = _wind.GetComponent<Wind>();
                            windAux.setTag("Wind");
                            windAux.setAbility(_wind);
                            windAux.setSize(_windCapacity);
                            windAux.setTimer(3f);
                            windAux.setCast(true);
                            windAux.setCoolDown(1f);

                            SuperWind superWindAux = _superWind.GetComponent<SuperWind>();
                            superWindAux.setTag("SuperWind");
                            superWindAux.setAbility(_superWind);
                            superWindAux.setSize(_superAbilityCapacity);
                            superWindAux.setTimer(_superAbilityTimer);
                            superWindAux.setCast(true);
                            superWindAux.setCoolDown(4.5f);
                            
                            _windList.Add(windAux);
                            _windList.Add(superWindAux);
                            
                            _abilities.Add(_windList);
                            _maxPowers += 1;

                            if (lightningUpgrade)
                            {
                                _lightningList = new List<Ability.Ability>();

                                Lightning lightningAux = _lightning.GetComponent<Lightning>();
                                lightningAux.setTag("Lightning");
                                lightningAux.setAbility(_lightning);
                                lightningAux.setSize(_lightningCapacity);
                                lightningAux.setTimer(0.5f);
                                lightningAux.setCast(true);
                                lightningAux.setCoolDown(2.5f);

                                SuperLightning superLightningAux = _superLightning.GetComponent<SuperLightning>();
                                superLightningAux.setTag("SuperLightning");
                                superLightningAux.setAbility(_superLightning);
                                superLightningAux.setSize(_superAbilityCapacity);
                                superLightningAux.setTimer(_superAbilityTimer);
                                superLightningAux.setCast(true);
                                superLightningAux.setCoolDown(4f);

                                _lightningList.Add(lightningAux);
                                _lightningList.Add(superLightningAux);
                                
                                _abilities.Add(_lightningList);
                                _maxPowers += 1;
                            }
                        }
                    }
                }
                
                
                abilityDictionary = new Dictionary<string, Queue<GameObject>>();
            
                foreach (List<Ability.Ability> ability in _abilities)
                {
                    foreach (Ability.Ability specificAbility in ability)
                    {
                        Queue<GameObject> abilityPool = new Queue<GameObject>();

                        for (int i = 0; i < specificAbility.getSize(); i++)
                        {
                            GameObject obj = Instantiate(specificAbility.getAbility());
                            obj.SetActive(false);
                            abilityPool.Enqueue(obj);
                        }
                
                        abilityDictionary.Add(specificAbility.getTag(), abilityPool);
                    }
                }
            }
        }
        
        private void ChoosePower()
        {
            if (_maxPowers != 0)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    _powerNum += 1;
                    if (_powerNum == _maxPowers)
                    {
                        _powerNum = 0;
                    }
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    _powerNum -= 1;
                    if (_powerNum == -1)
                    {
                        _powerNum = _maxPowers - 1;
                    }
                }
            }
        }
        
        private void UseNormalOrSuperAbility()
        {
            if (_maxPowers > 0)
            {
                
                
                if (Input.GetMouseButton(0) && _useSuperAbility)
                {
                    _holdTime -= Time.deltaTime;

                    if (_holdTime <= 0)
                    {
                        _useSuperAbility = false;
                        UsePower(true);
                    }
                        
                }
                else if (Input.GetMouseButtonUp(0) && _holdTime > 0)
                {
                    UsePower(false);
                }
                if (!Input.GetMouseButton(0))
                {
                    _useSuperAbility = true;
                    _holdTime = 2f;
                }
            }
        }
        
        private void UsePower(bool holded)
        {
            List<Ability.Ability> ability = new List<Ability.Ability>();

            int num;
            
            if (holded)
            {
                num = 1;
            }
            else
            {
                num = 0;
            }
            
            string tag = "";

            switch (_powerNum)
            {
                case 0:
                    ability = _abilities[_powerNum];
                    tag = ability[num].getTag();
                    
                    break;
                    
                case 1:
                    ability = _abilities[_powerNum];
                    tag = ability[num].getTag();
                    break;
                    
                case 2:
                    ability = _abilities[_powerNum];
                    tag = ability[num].getTag(); 
                    break;
                    
                case 3:
                    ability = _abilities[_powerNum];
                    tag = ability[num].getTag();
                    break;
                    
                case 4:
                    ability = _abilities[_powerNum];
                    tag = ability[num].getTag();
                    break;
                        
            }

            CastAbility(ability, tag, holded);
        }
        
        private void CastAbility(List<Ability.Ability> ability, string tag, bool holded)
        {
            GameObject abilityToSpawn;
            
            _abilityPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _abilityPos.z = 0f;

            if (transform.position.x < _abilityPos.x)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }

            abilityToSpawn = spawnAbility(tag);

            if (_powerNum == 0)
            {
                if (!holded)
                {
                    abilityToSpawn.GetComponent<Rock>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
                }
                else
                {
                    abilityToSpawn.GetComponent<SuperRock>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
                }
            }
            else if (_powerNum == 1)
            {
                if (!holded)
                {
                    abilityToSpawn.GetComponent<Water>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
                }
                else
                {
                    abilityToSpawn.GetComponent<SuperWater>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
                }
            } 
            else if (_powerNum == 2)
            {
                if (!holded)
                {
                    abilityToSpawn.GetComponent<Fire>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
                }
                else
                {
                    abilityToSpawn.GetComponent<SuperFire>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
                }
            } 
            else if (_powerNum == 3)
            {
                if (!holded)
                {
                    abilityToSpawn.GetComponent<Wind>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
                }
                else
                {
                    abilityToSpawn.GetComponent<SuperWind>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
                }
            }
            else if (_powerNum == 4)
            {
                if (!holded)
                {
                    abilityToSpawn.GetComponent<Lightning>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
                }
                else
                {
                    abilityToSpawn.GetComponent<SuperLightning>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
                }
            }

            /*if (ability[num].getTimer() > 0.3f)
            {
                abilityToSpawn = spawnAbility(tag);
                abilityToSpawn.transform.position = new Vector3( transform.position.x + direction,  transform.position.y, 0);
                abilityToSpawn.transform.rotation = targetRotation;
                abilityToSpawn.GetComponent<Rigidbody2D>().AddForce(new Vector2(_abilityPos.x - transform.position.x + direction, _abilityPos.y - transform.position.y).normalized * 300);
                StartCoroutine(AbilityDisappear(ability[num].getTimer(), abilityToSpawn));
                StartCoroutine(AbilityCooldown(ability[num].getTimer(), abilityToSpawn));
            }
            else
            {
                _abilityRange = (transform.position - _abilityPos).magnitude;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, (_abilityPos - transform.position).normalized, _abilityRange, LayerMask.GetMask("Tilemap1", "Tilemap2"));

                if (Mathf.Abs(_abilityRange) < _maxAbilityRange && !hit)
                {
                    abilityToSpawn = spawnAbility(tag);
                
                    if (ability[num].getTimer() != 0)
                    {
                        StartCoroutine(AbilityDisappear(ability[num].getTimer(), abilityToSpawn));
                    }

                    abilityToSpawn.transform.position = _abilityPos;
                    abilityToSpawn.transform.rotation = Quaternion.identity;
                }
            }*/
        }
        
        private GameObject spawnAbility(string tag)
        {
            GameObject abilityToSpawn;
            abilityToSpawn = abilityDictionary[tag].Dequeue();
            abilityToSpawn.SetActive(true);
            abilityDictionary[tag].Enqueue(abilityToSpawn);

            return abilityToSpawn;
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
                    Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, _posCamera, Time.deltaTime * _speed);
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
                    rockUpgrade = true;
                    break;
                
                case Abilities.WATER:
                    waterUpgrade = true;
                    break;
                
                case Abilities.FIRE:
                    fireUpgrade = true;
                    break;
                
                case Abilities.WIND:
                    windUpgrade = true;
                    break;
                
                case Abilities.LIGHTNING:
                    lightningUpgrade = true;
                    break;
            }
        }
        
    }
}