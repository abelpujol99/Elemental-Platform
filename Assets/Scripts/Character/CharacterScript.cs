using System;
using System.Collections;
using System.Collections.Generic;
using Ability.Abilities;
using System.Reflection;
using System.Security.Cryptography;
using CameraController;
using Ability;
using Ability.Abilities.Ninja;
using Ability.Abilities.Normal;
using Ability.Abilities.Super;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Character
{
    public class CharacterScript : MonoBehaviour
    {
        public enum Abilities {JUMP_UPGRADE, DOUBLE_JUMP_UPGRADE, ROCK, WATER, FIRE, WIND, LIGHTNING, SUPER_ROCK, SUPER_WATER, SUPER_FIRE, SUPER_WIND, SUPER_LIGHTNING}

        public static bool isGround,
            shurikenUpgrade,
            jumpUpgrade,
            doubleJumpUpgrade,
            rockUpgrade,
            waterUpgrade,
            fireUpgrade,
            windUpgrade,
            lightningUpgrade,
            superRockUpgrade,
            superWaterUpgrade,
            superFireUpgrade,
            superWindUpgrade,
            superLightningUpgrade;
        
        public static float _maxAbilityRange = 1;

        private static int _shurikenCapacity = 3, 
            _rockCapacity = 3,
            _waterCapacity = 3,
            _fireCapacity = 3,
            _windCapacity = 3,
            _lightningCapacity = 3,
            _superAbilityCapacity = 1; 

        private static float holdTime = 0.75f;

        private static float _shurikenTimer = 0f,
            _rockTimer = 0f,
            _waterTimer = 1f,
            _fireTimer = 2f,
            _windTimer = 3f,
            _lightningTimer = 0.5f,
            _shurikenCooldown = 1f,
            _rockCooldown = 2f,
            _waterCooldown = 2f,
            _fireCooldown = 3f,
            _windCooldown = 1f,
            _lightningCooldown = 2.5f,
            _superRockCooldown = 5f,
            _superWaterCooldown = 4f,
            _superFireCooldown = 5f,
            _superWindCooldown = 4.5f,
            _superLightningCooldown = 4f;
        
        public Animator animator;

        [SerializeField] private GameObject _shuriken,
            _rock,
            _water,
            _fire,
            _wind,
            _lightning,
            _superShuriken,
            _superRock,
            _superWater,
            _superFire,
            _superWind,
            _superLightning,
            _visualTimer,
            _abilityRangeCircle;

        [SerializeField] private CooldownActiveAbility _cooldownActiveAbility;

        private SpriteRenderer _spriteRenderer;

        private Rigidbody2D _rb2D;

        private SpriteRenderer _abilityRangeCircleSprite;
        
        private Vector3 _abilityPos;

        private RaycastHit2D _leftLeg;
        private RaycastHit2D _rightLeg;
        private RaycastHit2D _hit;

        private List<List<Ability.Ability>> _abilities;
        
        private Dictionary<string, Queue<GameObject>> _abilityDictionary;

        private List<Ability.Ability> _shurikenList,
            _rockList,
            _waterList,
            _fireList,
            _windList,
            _lightningList,
            _superShurikenList,
            _superRockList,
            _superWaterList,
            _superWindList,
            _superLightningList;

        private Dictionary<int, List<DateTime>> _allAbilitiesCooldowns;

        private List<DateTime> _shurikenCooldownList,
            _rockCooldownList,
            _waterCooldownList,
            _fireCooldownList,
            _windCooldownList,
            _lightningCooldownList;
        
        private float _moveSpeed = 2;
        private float _jumpSpeed = 3;
        private float _doubleJumpSpeed = 2.5f;
        private float _fallMultiplier = 0.5f;
        private float _lowJumpMultiplier = 1f;
        private float _abilityRange;
        private float _superAbilityTimer = 0.3f;
        private float _auxHoldTime;

        private bool _run, _jump, _fall, _doubleJump, _canDoubleJump, _useSuperAbility = true;

        private int _maxPowers, _powerNum, _auxPowerNum;

        private DateTime _initialTime;

        void Start()
        {
            shurikenUpgrade = true;
            rockUpgrade = true;
            waterUpgrade = true;
            fireUpgrade = true;
            windUpgrade = true;
            lightningUpgrade = true;
            _abilityRangeCircle.GetComponent<MaxAbilityRange>().SetScale();
            _abilityRangeCircleSprite = _abilityRangeCircle.GetComponent<SpriteRenderer>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb2D = GetComponent<Rigidbody2D>();
            _cooldownActiveAbility = _cooldownActiveAbility.GetComponent<CooldownActiveAbility>();
            _auxHoldTime = holdTime;
            _powerNum = 1;
            _auxPowerNum = 0;
            SetPowers();
            //jumpUpgrade = true;
            //doubleJumpUpgrade = true;
            _initialTime = DateTime.Now;
            _initialTime = new DateTime(_initialTime.Year, _initialTime.Month, _initialTime.Day, _initialTime.Hour,
                _initialTime.Minute - 1, _initialTime.Second);
        }

        void Update()
        {
            if (_powerNum != _auxPowerNum)
            {
                _cooldownActiveAbility.ShowAbilities(_powerNum, _abilities[_powerNum][0].getSize());
            }
            
            _auxPowerNum = _powerNum;

            if (Input.GetKey(KeyCode.R))
            {
                GetComponent<PlayerRespawn>().RestartLevel();
            }
            
            Jump();
            
            ChoosePowerWithButton();
            
            if (!Input.GetMouseButton(1))
            {
                _abilityRangeCircleSprite.enabled = false;
                
                if (Input.GetMouseButtonDown(0))
                {
                    _abilityPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _abilityPos.z = 0f;
                    
                    CastAbility("Shuriken", false);
                }
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
            if (isGround)
            {
                _rb2D.velocity += Vector2.up * Physics2D.gravity * 0;
            }
            else if (_rb2D.velocity.y < 0)
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

                _allAbilitiesCooldowns = new Dictionary<int, List<DateTime>>();

                _shurikenCooldownList = new List<DateTime>();
                
                _shurikenList = new List<Ability.Ability>();

                Shuriken shurikenAux = _shuriken.GetComponent<Shuriken>();
                shurikenAux.setTag("Shuriken");
                shurikenAux.setAbility(_shuriken);
                shurikenAux.setSize(_shurikenCapacity);
                shurikenAux.setTimer(_shurikenTimer);
                shurikenAux.setCast(true);
                shurikenAux.setCoolDown(_shurikenCooldown);
                
                _shurikenList.Add(shurikenAux);
                
                _abilities.Add(_shurikenList);

                for (int i = 0; i < _shurikenCapacity; i++)
                {
                    _shurikenCooldownList.Add(_initialTime);
                }
                
                _allAbilitiesCooldowns.Add(0, _shurikenCooldownList);


                if (rockUpgrade)
                {

                    _rockList = new List<Ability.Ability>();

                    _rockCooldownList = new List<DateTime>();

                    Rock rockAux = _rock.GetComponent<Rock>();
                    rockAux.setTag("Rock");
                    rockAux.setAbility(_rock);
                    rockAux.setSize(_rockCapacity);
                    rockAux.setTimer(_rockTimer);
                    rockAux.setCast(true);
                    rockAux.setCoolDown(_rockCooldown);

                    SuperRock superRockAux = _superRock.GetComponent<SuperRock>();
                    superRockAux.setTag("SuperRock");
                    superRockAux.setAbility(_superRock);
                    superRockAux.setSize(_superAbilityCapacity);
                    superRockAux.setTimer(_superAbilityTimer);
                    superRockAux.setCast(true);
                    superRockAux.setCoolDown(_superRockCooldown);

                    _rockList.Add(rockAux);
                    _rockList.Add(superRockAux);

                    _abilities.Add(_rockList);

                    for (int i = 0; i <= _rockCapacity; i++)
                    {
                        _rockCooldownList.Add(_initialTime);
                    }

                    _allAbilitiesCooldowns.Add(1, _rockCooldownList);

                    _maxPowers += 1;

                    if (waterUpgrade)
                    {
                        _waterList = new List<Ability.Ability>();

                        _waterCooldownList = new List<DateTime>();

                        Water waterAux = _water.GetComponent<Water>();
                        waterAux.setTag("Water");
                        waterAux.setAbility(_water);
                        waterAux.setSize(_waterCapacity);
                        waterAux.setTimer(_waterTimer);
                        waterAux.setCast(true);
                        waterAux.setCoolDown(_waterCooldown);

                        SuperWater superWaterAux = _superWater.GetComponent<SuperWater>();
                        superWaterAux.setTag("SuperWater");
                        superWaterAux.setAbility(_superWater);
                        superWaterAux.setSize(_superAbilityCapacity);
                        superWaterAux.setTimer(_superAbilityTimer);
                        superWaterAux.setCast(true);
                        superWaterAux.setCoolDown(_superWaterCooldown);

                        _waterList.Add(waterAux);
                        _waterList.Add(superWaterAux);

                        _abilities.Add(_waterList);

                        for (int i = 0; i <= _waterCapacity; i++)
                        {
                            _waterCooldownList.Add(_initialTime);
                        }

                        _allAbilitiesCooldowns.Add(2, _waterCooldownList);

                        _maxPowers += 1;

                        if (fireUpgrade)
                        {
                            _fireList = new List<Ability.Ability>();

                            _fireCooldownList = new List<DateTime>();

                            Fire fireAux = _fire.GetComponent<Fire>();
                            fireAux.setTag("Fire");
                            fireAux.setAbility(_fire);
                            fireAux.setSize(_fireCapacity);
                            fireAux.setTimer(_fireTimer);
                            fireAux.setCast(true);
                            fireAux.setCoolDown(_fireCooldown);

                            SuperFire superFireAux = _superFire.GetComponent<SuperFire>();
                            superFireAux.setTag("SuperFire");
                            superFireAux.setAbility(_superFire);
                            superFireAux.setSize(_superAbilityCapacity);
                            superFireAux.setTimer(_superAbilityTimer);
                            superFireAux.setCast(true);
                            superFireAux.setCoolDown(_superFireCooldown);

                            _fireList.Add(fireAux);
                            _fireList.Add(superFireAux);

                            _abilities.Add(_fireList);

                            for (int i = 0; i <= _fireCapacity; i++)
                            {
                                _fireCooldownList.Add(_initialTime);
                            }

                            _allAbilitiesCooldowns.Add(3, _fireCooldownList);

                            _maxPowers += 1;

                            if (windUpgrade)
                            {

                                _windList = new List<Ability.Ability>();

                                _windCooldownList = new List<DateTime>();

                                Wind windAux = _wind.GetComponent<Wind>();
                                windAux.setTag("Wind");
                                windAux.setAbility(_wind);
                                windAux.setSize(_windCapacity);
                                windAux.setTimer(_windTimer);
                                windAux.setCast(true);
                                windAux.setCoolDown(_windCooldown);

                                SuperWind superWindAux = _superWind.GetComponent<SuperWind>();
                                superWindAux.setTag("SuperWind");
                                superWindAux.setAbility(_superWind);
                                superWindAux.setSize(_superAbilityCapacity);
                                superWindAux.setTimer(_superAbilityTimer);
                                superWindAux.setCast(true);
                                superWindAux.setCoolDown(_superWindCooldown);

                                _windList.Add(windAux);
                                _windList.Add(superWindAux);

                                _abilities.Add(_windList);
                                for (int i = 0; i <= _windCapacity; i++)
                                {
                                    _windCooldownList.Add(_initialTime);
                                }

                                _allAbilitiesCooldowns.Add(4, _windCooldownList);

                                _maxPowers += 1;

                                if (lightningUpgrade)
                                {
                                    _lightningList = new List<Ability.Ability>();

                                    _lightningCooldownList = new List<DateTime>();

                                    Lightning lightningAux = _lightning.GetComponent<Lightning>();
                                    lightningAux.setTag("Lightning");
                                    lightningAux.setAbility(_lightning);
                                    lightningAux.setSize(_lightningCapacity);
                                    lightningAux.setTimer(_lightningTimer);
                                    lightningAux.setCast(true);
                                    lightningAux.setCoolDown(_lightningCooldown);

                                    SuperLightning superLightningAux =
                                        _superLightning.GetComponent<SuperLightning>();
                                    superLightningAux.setTag("SuperLightning");
                                    superLightningAux.setAbility(_superLightning);
                                    superLightningAux.setSize(_superAbilityCapacity);
                                    superLightningAux.setTimer(_superAbilityTimer);
                                    superLightningAux.setCast(true);
                                    superLightningAux.setCoolDown(_superLightningCooldown);

                                    _lightningList.Add(lightningAux);
                                    _lightningList.Add(superLightningAux);

                                    _abilities.Add(_lightningList);
                                    
                                    for (int i = 0; i <= _lightningCapacity; i++)
                                    {
                                        _lightningCooldownList.Add(_initialTime);
                                    }

                                    _allAbilitiesCooldowns.Add(5, _lightningCooldownList);

                                    _maxPowers += 1;
                                }
                            }
                        }
                    }
                }

                _abilityDictionary = new Dictionary<string, Queue<GameObject>>();
                
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
                        
                        _abilityDictionary.Add(specificAbility.getTag(), abilityPool);
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

                _abilityPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _abilityPos.z = 0f;
                    
                _abilityRange = (_abilityPos - transform.position).magnitude;
                
                if (Input.GetMouseButton(0) && _useSuperAbility)
                {
                    
                    _hit = Physics2D.Raycast(transform.position, _abilityPos - transform.position, _abilityRange, LayerMask.GetMask("Tilemap1", "Tilemap2"));

                    if (_hit)
                    {
                        visualComponent.ResetFillAmount();
                        _visualTimer.SetActive(false);
                    }
                    else if (holdTime == _auxHoldTime && _maxAbilityRange >= _abilityRange && !_hit)
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

                    if (holdTime <= 0 && _maxAbilityRange >= _abilityRange && !_hit)
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
                    
                    if (!(_maxAbilityRange < _abilityRange && _powerNum == 1))
                    {
                        UsePower(false);
                    }
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

            CastAbility(tag, holded);
        }
        
        private void CastAbility(string tag, bool holded)
        {
            GameObject abilityToSpawn;
            
            abilityToSpawn = spawnAbility(tag);

            Ability.Ability abilityComponent = abilityToSpawn.GetComponent<Ability.Ability>();

            if (abilityComponent.isCast())
            {
                if (!(_powerNum == 1 && _hit))
                {
                    abilityToSpawn.SetActive(false);
                }
                
                if (transform.position.x < _abilityPos.x)
                {
                    _spriteRenderer.flipX = false;
                }
                else
                {
                    _spriteRenderer.flipX = true;
                }

                StartCoroutine(AbilityCooldown(abilityComponent));
                
                if (tag == "Shuriken")
                {
                    abilityComponent.abilityUtility(abilityToSpawn, _abilityPos, transform.position, _maxAbilityRange);
                }
                else
                {
                    abilityComponent.abilityUtility(abilityToSpawn, _abilityPos, transform.position, _maxAbilityRange);
                }
                    
                _cooldownActiveAbility.UpdateCooldown(_allAbilitiesCooldowns[_powerNum], holded, _abilities[_powerNum][0].getCooldown(), _abilities[_powerNum][1].getCooldown());

                if (abilityComponent.getTimer() > 0)
                {
                    StartCoroutine(AbilityDisappear(abilityComponent.getTimer(), abilityToSpawn));
                }
                
            }
        }
        
        private GameObject spawnAbility(string tag)
        {
            GameObject abilityToSpawn;

            abilityToSpawn = _abilityDictionary[tag].Dequeue();
            _abilityDictionary[tag].Enqueue(abilityToSpawn);

            return abilityToSpawn;
        }

        private IEnumerator AbilityCooldown(Ability.Ability abilityComponent)
        {
            yield return new WaitForSeconds(abilityComponent.getCooldown());
            abilityComponent.setCast(true);
            
        }

        private IEnumerator AbilityDisappear(float timer, GameObject ability)
        {
            yield return new WaitForSeconds(timer);
            ability.SetActive(false);
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
                    SetPowers();
                    break;
                
                case Abilities.WATER:
                    waterUpgrade = true;
                    SetPowers();
                    break;
                
                case Abilities.FIRE:
                    fireUpgrade = true;
                    SetPowers();
                    break;
                
                case Abilities.WIND:
                    windUpgrade = true;
                    SetPowers();
                    break;
                
                case Abilities.LIGHTNING:
                    lightningUpgrade = true;
                    SetPowers();
                    break;
                
                case Abilities.SUPER_ROCK:
                    superRockUpgrade = true;
                    break;
                
                case Abilities.SUPER_WATER:
                    superWaterUpgrade = true;
                    SetPowers();
                    break;
                
                case Abilities.SUPER_FIRE:
                    superFireUpgrade = true;
                    SetPowers();
                    break;
                
                case Abilities.SUPER_WIND:
                    superWindUpgrade = true;
                    SetPowers();
                    break;
                
                case Abilities.SUPER_LIGHTNING:
                    superLightningUpgrade = true;
                    SetPowers();
                    break;
            }
        }
    }
}