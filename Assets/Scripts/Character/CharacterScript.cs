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
using UnityEngine.UI;

namespace Character
{
    public class CharacterScript : MonoBehaviour
    {
        public enum Abilities {JUMP_UPGRADE, DOUBLE_JUMP_UPGRADE, ROCK, WATER, FIRE, WIND, LIGHTNING, SUPER_ROCK, SUPER_WATER, SUPER_FIRE, SUPER_WIND, SUPER_LIGHTNING}

        public static bool shurikenUpgrade, jumpUpgrade, doubleJumpUpgrade, rockUpgrade, waterUpgrade, fireUpgrade, windUpgrade, lightningUpgrade, superRockUpgrade, superWaterUpgrade, superFireUpgrade, superWindUpgrade, superLightningUpgrade;
        
        public static float _maxAbilityRange = 1;

        private static float holdTime;
        
        public Animator animator;
        
        public bool isGround;

        [SerializeField] private GameObject _shuriken, _rock, _water, _fire, _wind, _lightning, _superShuriken, _superRock, _superWater, _superFire, _superWind, _superLightning, _visualTimer, _abilityRangeCircle;

        private SpriteRenderer _spriteRenderer;

        private Rigidbody2D _rb2D;

        private Camera _camera;

        private CameraMoves _cameraComponent;

        private SpriteRenderer _abilityRangeCircleSprite;
        
        private Vector3 _abilityPos, _posCamera, _initialPosCamera;

        private RaycastHit2D hit;

        private List<List<Ability.Ability>> _abilities;
        
        private Dictionary<string, Queue<GameObject>> abilityDictionary;

        private List<Ability.Ability> _shurikenList , _rockList, _waterList, _fireList, _windList, _lightningList, _superShurikenList, _superRockList, _superWaterList, _superWindList, _superLightningList;
        
        private float _moveSpeed = 2;
        private float _jumpSpeed = 3;
        private float _doubleJumpSpeed = 2.5f;
        private float _fallMultiplier = 0.5f;
        private float _lowJumpMultiplier = 1f;
        private float _maximumZoomIn = 0.54f;
        private float _maximumZoomOut;
        private float _abilityRange;
        private float _superAbilityTimer = 0.3f;
        private float _auxHoldTime;
        private float _cooldownRock = 2f;
        private float _cooldownWater = 2f;
        private float _cooldownFire = 3f;
        private float _cooldownWind = 1f;
        private float _cooldownLightning = 2.5f;
        private float _cooldownSuperRock = 5f;
        private float _cooldownSuperWater = 4f;
        private float _cooldownSuperFire = 5f;
        private float _cooldownSuperWind = 4.5f;
        private float _cooldownSuperLightning = 4f;
        
        private bool _run, _jump, _fall, _doubleJump, _canDoubleJump, _useSuperAbility;

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
            shurikenUpgrade = true;
            rockUpgrade = true;
            waterUpgrade = true;
            fireUpgrade = true;
            windUpgrade = true;
            lightningUpgrade = true;
            holdTime = 0.75f;
            _abilityRangeCircle.GetComponent<MaxAbilityRange>().SetScale();
            _camera = Camera.main;
            _abilityRangeCircleSprite = _abilityRangeCircle.GetComponent<SpriteRenderer>();
            _cameraComponent = _camera.GetComponent<CameraMoves>();
            _initialPosCamera = Camera.main.transform.position;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _maximumZoomOut = Camera.main.orthographicSize;
            _posCamera = Camera.main.transform.position;
            _rb2D = GetComponent<Rigidbody2D>();
            _powerNum = 1;
            _auxHoldTime = holdTime;
            SetPowers();
            jumpUpgrade = true;
            doubleJumpUpgrade = true;
            _useSuperAbility = true;
        }

        void Update()
        {
            Jump();
            ChoosePowerWithButton();
            if (!Input.GetMouseButton(1))
            {
                _abilityRangeCircleSprite.enabled = false;
                
                if (Input.GetMouseButtonDown(0))
                {
                    _abilityPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _abilityPos.z = 0f;
                    
                    CastAbility("Shuriken");
                }
                Zoom();
            }
            else
            {
                _abilityRangeCircleSprite.enabled = true;
                ChoosePowerWithScroll();
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
            if (shurikenUpgrade)
            {
                _abilities = new List<List<Ability.Ability>>();
                
                _shurikenList = new List<Ability.Ability>();

                Shuriken shurikenAux = _shuriken.GetComponent<Shuriken>();
                shurikenAux.setTag("Shuriken");
                shurikenAux.setAbility(_shuriken);
                shurikenAux.setSize(1);
                shurikenAux.setTimer(0f);
                shurikenAux.setCast(true);
                shurikenAux.setCoolDown(1f);
                
                _shurikenList.Add(shurikenAux);
                
                _abilities.Add(_shurikenList);
                _maxPowers += 1;

                if (rockUpgrade)
                {

                    _rockList = new List<Ability.Ability>();

                    Rock rockAux = _rock.GetComponent<Rock>();
                    rockAux.setTag("Rock");
                    rockAux.setAbility(_rock);
                    rockAux.setSize(2);
                    rockAux.setTimer(0f);
                    rockAux.setCast(true);
                    rockAux.setCoolDown(_cooldownRock);

                    SuperRock superRockAux = _superRock.GetComponent<SuperRock>();
                    superRockAux.setTag("SuperRock");
                    superRockAux.setAbility(_superRock);
                    superRockAux.setSize(_superAbilityCapacity);
                    superRockAux.setTimer(_superAbilityTimer);
                    superRockAux.setCast(true);
                    superRockAux.setCoolDown(_cooldownSuperRock);

                    _rockList.Add(rockAux);
                    _rockList.Add(superRockAux);

                    _abilities.Add(_rockList);
                    _maxPowers += 1;

                    if (waterUpgrade)
                    {
                        _waterList = new List<Ability.Ability>();

                        ThrowableAbility waterAux = _water.GetComponent<ThrowableAbility>();
                        waterAux.setTag("Water");
                        waterAux.setAbility(_water);
                        waterAux.setSize(_waterCapacity);
                        waterAux.setTimer(1f);
                        waterAux.setCast(true);
                        waterAux.setCoolDown(_cooldownWater);

                        StaticAbility superWaterAux = _superWater.GetComponent<StaticAbility>();
                        superWaterAux.setTag("SuperWater");
                        superWaterAux.setAbility(_superWater);
                        superWaterAux.setSize(_superAbilityCapacity);
                        superWaterAux.setTimer(_superAbilityTimer);
                        superWaterAux.setCast(true);
                        superWaterAux.setCoolDown(_cooldownSuperWater);

                        _waterList.Add(waterAux);
                        _waterList.Add(superWaterAux);

                        _abilities.Add(_waterList);
                        _maxPowers += 1;

                        if (fireUpgrade)
                        {
                            _fireList = new List<Ability.Ability>();

                            ThrowableAbility fireAux = _fire.GetComponent<ThrowableAbility>();
                            fireAux.setTag("Fire");
                            fireAux.setAbility(_fire);
                            fireAux.setSize(_fireCapacity);
                            fireAux.setTimer(2f);
                            fireAux.setCast(true);
                            fireAux.setCoolDown(_cooldownFire);

                            StaticAbility superFireAux = _superFire.GetComponent<StaticAbility>();
                            superFireAux.setTag("SuperFire");
                            superFireAux.setAbility(_superFire);
                            superFireAux.setSize(_superAbilityCapacity);
                            superFireAux.setTimer(_superAbilityTimer);
                            superFireAux.setCast(true);
                            superFireAux.setCoolDown(_cooldownSuperFire);

                            _fireList.Add(fireAux);
                            _fireList.Add(superFireAux);

                            _abilities.Add(_fireList);
                            _maxPowers += 1;

                            if (windUpgrade)
                            {

                                _windList = new List<Ability.Ability>();

                                ThrowableAbility windAux = _wind.GetComponent<ThrowableAbility>();
                                windAux.setTag("Wind");
                                windAux.setAbility(_wind);
                                windAux.setSize(_windCapacity);
                                windAux.setTimer(3f);
                                windAux.setCast(true);
                                windAux.setCoolDown(_cooldownWind);

                                StaticAbility superWindAux = _superWind.GetComponent<StaticAbility>();
                                superWindAux.setTag("SuperWind");
                                superWindAux.setAbility(_superWind);
                                superWindAux.setSize(_superAbilityCapacity);
                                superWindAux.setTimer(_superAbilityTimer);
                                superWindAux.setCast(true);
                                superWindAux.setCoolDown(_cooldownSuperWind);

                                _windList.Add(windAux);
                                _windList.Add(superWindAux);

                                _abilities.Add(_windList);
                                _maxPowers += 1;

                                if (lightningUpgrade)
                                {
                                    _lightningList = new List<Ability.Ability>();

                                    ThrowableAbility lightningAux = _lightning.GetComponent<ThrowableAbility>();
                                    lightningAux.setTag("Lightning");
                                    lightningAux.setAbility(_lightning);
                                    lightningAux.setSize(_lightningCapacity);
                                    lightningAux.setTimer(0.5f);
                                    lightningAux.setCast(true);
                                    lightningAux.setCoolDown(_cooldownLightning);

                                    StaticAbility superLightningAux = _superLightning.GetComponent<StaticAbility>();
                                    superLightningAux.setTag("SuperLightning");
                                    superLightningAux.setAbility(_superLightning);
                                    superLightningAux.setSize(_superAbilityCapacity);
                                    superLightningAux.setTimer(_superAbilityTimer);
                                    superLightningAux.setCast(true);
                                    superLightningAux.setCoolDown(_cooldownSuperLightning);

                                    _lightningList.Add(lightningAux);
                                    _lightningList.Add(superLightningAux);

                                    _abilities.Add(_lightningList);
                                    _maxPowers += 1;
                                }
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

        private void ChoosePowerWithButton()
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                _powerNum = 1;
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                _powerNum = 2;
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                _powerNum = 3;
            }
            else if (Input.GetKey(KeyCode.Alpha4))
            {
                _powerNum = 4;
            }
            else if (Input.GetKey(KeyCode.Alpha5))
            {
                _powerNum = 5;
            }
        }
        
        private void ChoosePowerWithScroll()
        {
            if (_maxPowers != 0)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    _powerNum += 1;
                    if (_powerNum == _maxPowers + 1)
                    {
                        _powerNum = 1;
                    }
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    _powerNum -= 1;
                    if (_powerNum == 0)
                    {
                        _powerNum = _maxPowers;
                    }
                }
            }
        }
        
        private void UseNormalOrSuperAbility()
        {
            if (_maxPowers > 0)
            {
                SuperAbilityCastTime visualComponent = _visualTimer.GetComponent<SuperAbilityCastTime>();
                
                if (Input.GetMouseButton(0) && _useSuperAbility)
                {
                    _abilityPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _abilityPos.z = 0f;
                    
                    _abilityRange = (_abilityPos - transform.position).magnitude;
                    
                    hit = Physics2D.Raycast(transform.position, _abilityPos - transform.position, _abilityRange, LayerMask.GetMask("Tilemap1", "Tilemap2"));

                    if (hit)
                    {
                        visualComponent.ResetFillAmount();
                        _visualTimer.SetActive(false);
                    }
                    else if (holdTime == _auxHoldTime && _maxAbilityRange >= _abilityRange && !hit)
                    {
                        _visualTimer.SetActive(true);
                        visualComponent.HoldedTime(holdTime);
                    }
                    else if (_maxAbilityRange <= _abilityRange)
                    {
                        visualComponent.ResetFillAmount();
                        _visualTimer.SetActive(false);
                        
                    }
                    holdTime -= Time.deltaTime;

                    if (holdTime <= 0 && _maxAbilityRange >= _abilityRange && !hit)
                    {
                        _useSuperAbility = false;
                        UsePower(true);
                        visualComponent.ResetFillAmount();
                        _visualTimer.SetActive(false);
                    }
                        
                }
                else if (Input.GetMouseButtonUp(0) && holdTime > 0)
                {
                    visualComponent.ResetFillAmount();
                    _visualTimer.SetActive(false);
                    UsePower(false);
                }
                if (!Input.GetMouseButton(0))
                {
                    _useSuperAbility = true;
                    holdTime = _auxHoldTime;
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
                    
                case 5:
                    ability = _abilities[_powerNum];
                    tag = ability[num].getTag();
                    break;
                        
            }

            CastAbility(tag);
        }
        
        private void CastAbility(string tag)
        {
            GameObject abilityToSpawn;

            abilityToSpawn = spawnAbility(tag);

            Ability.Ability abilityComponent = abilityToSpawn.GetComponent<Ability.Ability>();
            

            if (abilityComponent.isCast())
            {
                if (transform.position.x < _abilityPos.x)
                {
                    _spriteRenderer.flipX = false;
                }
                else
                {
                    _spriteRenderer.flipX = true;
                }

                StartCoroutine(AbilityCooldown(abilityComponent, abilityToSpawn));
                
                if (tag == "Shuriken")
                {
                    abilityComponent.abilityUtility(abilityToSpawn, _abilityPos, transform.position, _maxAbilityRange);
                }
                else if (_powerNum == 1)
                {
                    abilityComponent.abilityUtility(abilityToSpawn, _abilityPos, transform.position, _maxAbilityRange);
                }
                else if (_powerNum == 2)
                {
                    abilityComponent.abilityUtility(abilityToSpawn, _abilityPos, transform.position, _maxAbilityRange);
                } 
                else if (_powerNum == 3)
                {
                    abilityComponent.abilityUtility(abilityToSpawn, _abilityPos, transform.position, _maxAbilityRange);
                } 
                else if (_powerNum == 4)
                {
                    abilityComponent.abilityUtility(abilityToSpawn, _abilityPos, transform.position, _maxAbilityRange);
                }
                else if (_powerNum == 5)
                {
                    abilityComponent.abilityUtility(abilityToSpawn, _abilityPos, transform.position, _maxAbilityRange);
                }

                if (abilityComponent.getTimer() > 0)
                {
                    StartCoroutine(AbilityDisappear(abilityComponent.getTimer(), abilityToSpawn));
                }
                
            }
        }
        
        private GameObject spawnAbility(string tag)
        {
            GameObject abilityToSpawn;

            abilityToSpawn = abilityDictionary[tag].Dequeue();
            abilityToSpawn.SetActive(false);
            abilityDictionary[tag].Enqueue(abilityToSpawn);

            return abilityToSpawn;
        }

        private IEnumerator AbilityCooldown(Ability.Ability abilityComponent, GameObject ability)
        {
            yield return new WaitForSeconds(abilityComponent.getTimer());
            abilityComponent.setCast(true);
            
        }

        private IEnumerator AbilityDisappear(float timer, GameObject ability)
        {
            yield return new WaitForSeconds(timer);
            ability.SetActive(false);
        }
        
        private void Zoom()
        {
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            if (zoom > 0f)
            {
                if (_camera.orthographicSize > _maximumZoomIn)
                {
                    _camera.orthographicSize -= 0.1f;
                    _camera.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, _posCamera.z), Time.deltaTime * _speed);
                    //_cameraComponent.enabled = true;
                }
            } 
            else if (zoom < 0f)
            {
                if (_camera.orthographicSize < _maximumZoomOut)
                {
                    _camera.orthographicSize += 0.1f;
                    _camera.transform.position = Vector3.MoveTowards(Camera.main.transform.position, _posCamera, Time.deltaTime * _speed);
                    if (_camera.transform.position == _initialPosCamera)
                    {
                        //_cameraComponent.enabled = false;
                    }
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
                
                case Abilities.SUPER_ROCK:
                    superRockUpgrade = true;
                    break;
                
                case Abilities.SUPER_WATER:
                    superWaterUpgrade = true;
                    break;
                
                case Abilities.SUPER_FIRE:
                    superFireUpgrade = true;
                    break;
                
                case Abilities.SUPER_WIND:
                    superWindUpgrade = true;
                    break;
                
                case Abilities.SUPER_LIGHTNING:
                    superLightningUpgrade = true;
                    break;
            }
        }
        
    }
}