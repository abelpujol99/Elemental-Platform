using System;
using System.Collections;
using System.Collections.Generic;
using Ability.Abilities;
using Ability;
using Ability.Abilities.Ninja;
using Ability.Abilities.Normal;
using Ability.Abilities.Super;
using Ability.Abilities.Super.SuperRock;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Character
{
    public class CharacterScript : MonoBehaviour
    {
        public enum Abilities {JUMP_UPGRADE, DOUBLE_JUMP_UPGRADE, SHURIKEN, ROCK, WATER, FIRE, WIND, LIGHTNING}

        public static bool isGround,
            jumpUpgrade,
            doubleJumpUpgrade,
            shurikenUpgrade,
            rockUpgrade,
            waterUpgrade,
            fireUpgrade,
            windUpgrade,
            lightningUpgrade;
        
        public static float _maxAbilityRange = 1;

        private static int _shurikenCapacity = 3, 
            _rockCapacity = 2,
            _waterCapacity = 1,
            _fireCapacity = 1,
            _windCapacity = 1,
            _lightningCapacity = 1,
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
            _superRockCooldown = 10f,
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

        private Dictionary<int, List<float>> _allAbilitiesCooldowns;

        private List<float> _shurikenCooldownList,
            _rockCooldownList,
            _waterCooldownList,
            _fireCooldownList,
            _windCooldownList,
            _lightningCooldownList,
            _auxList,
            _auxSpecificAbilityList;
        
        private float _moveSpeed = 2;
        private float _jumpSpeed = 3;
        private float _doubleJumpSpeed = 2.5f;
        private float _fallMultiplier = 0.5f;
        private float _lowJumpMultiplier = 1f;
        private float _abilityRange;
        private float _superRockTimer = 2f;
        private float _superAbilityTimer = 0.3f;
        private float _auxHoldTime;

        private bool _run, _jump, _fall, _doubleJump, _canDoubleJump, _useSuperAbility = true;

        private int _maxPowers, _powerNum, _auxPowerNum;

        private float _initialTime;
        private float _actualTime;

        void Start()
        {
            _abilityRangeCircle.GetComponent<MaxAbilityRange>().SetScale();
            _abilityRangeCircleSprite = _abilityRangeCircle.GetComponent<SpriteRenderer>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb2D = GetComponent<Rigidbody2D>();
            _cooldownActiveAbility = _cooldownActiveAbility.GetComponent<CooldownActiveAbility>();
            _auxHoldTime = holdTime;
            _powerNum = 1;
            _auxPowerNum = 0;
            SetPowers();
            //_initialTime = Time.timeSinceLevelLoad - 30;
        }
        
        #region PowersSet

        private void SetShurikenAction()
        {
            _abilities = new List<List<Ability.Ability>>();

            _allAbilitiesCooldowns = new Dictionary<int, List<float>>();

            SetUniquePower(0, _shurikenList, _shurikenCooldownList, "Shuriken", _shuriken, _shurikenCapacity,
                _shurikenTimer, _shurikenCooldown);
        }

        private void SetRockAction()
        {
            SetDualPower(1, _rockList, _rockCooldownList, "Rock", _rock, _rockCapacity, _rockTimer,
                _rockCooldown, "SuperRock", _superRock, _superRockTimer, _superRockCooldown);
        }

        private void SetWaterAction()
        {
            SetDualPower(2, _waterList, _waterCooldownList, "Water", _water, _waterCapacity, _waterTimer,
                _waterCooldown, "SuperWater", _superWater, _superAbilityTimer, _superWaterCooldown);
        }

        private void SetFireAction()
        {
            SetDualPower(3, _fireList, _fireCooldownList, "Fire", _fire, _fireCapacity, _fireTimer,
                _fireCooldown, "SuperFire", _superFire, _superAbilityTimer, _superFireCooldown);
        }

        private void SetWindAction()
        {
            SetDualPower(4, _windList, _windCooldownList, "Wind", _wind, _windCapacity, _windTimer,
                _windCooldown, "SuperWind", _superWind, _superAbilityTimer, _superWindCooldown); 
        }

        private void SetLightningAction()
        {
            SetDualPower(5, _lightningList, _lightningCooldownList, "Lightning", _lightning,
                _lightningCapacity, _lightningTimer, _lightningCooldown, "SuperLightning",
                _superLightning, _superAbilityTimer, _superLightningCooldown);
        }
        
        #endregion PowersSet

        void Update()
        {

            if (Input.GetKey(KeyCode.R))
            {
                GetComponent<PlayerRespawn>().RestartLevel();
            }

            if (_powerNum != _auxPowerNum && _maxPowers > 0)
            {
                _cooldownActiveAbility.ShowAbilities(_powerNum, _abilities[_powerNum][0].getSize());
                /*_cooldownActiveAbility.ShowCooldown(_allAbilitiesCooldowns[_powerNum], _abilities[_powerNum][0].getCooldown(), _abilities[_powerNum][1].getCooldown(), false);*/
            }
            
            _auxPowerNum = _powerNum;
            
            Jump();
            
            ChoosePowerWithButton();
            
            if (!Input.GetMouseButton(1))
            {
                _abilityRangeCircleSprite.enabled = false;

                if (!Input.GetMouseButtonDown(0) || !shurikenUpgrade)
                {
                    return;
                }
                
                _abilityPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _abilityPos.z = 0f;
                    
                CastAbility("Shuriken", 0);
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

                //_allAbilitiesCooldowns = new Dictionary<int, List<float>>();

                SetUniquePower(0, _shurikenList, _shurikenCooldownList, "Shuriken", _shuriken, _shurikenCapacity,
                    _shurikenTimer, _shurikenCooldown);
                
                if (rockUpgrade)
                {
                    SetDualPower(1, _rockList, _rockCooldownList, "Rock", _rock, _rockCapacity, _rockTimer,
                        _rockCooldown, "SuperRock", _superRock, _superRockTimer, _superRockCooldown);
                
                    if (waterUpgrade)
                    {
                        SetDualPower(2, _waterList, _waterCooldownList, "Water", _water, _waterCapacity, _waterTimer,
                            _waterCooldown, "SuperWater", _superWater, _superAbilityTimer, _superWaterCooldown);
                
                        if (fireUpgrade)
                        {
                            SetDualPower(3, _fireList, _fireCooldownList, "Fire", _fire, _fireCapacity, _fireTimer,
                                _fireCooldown, "SuperFire", _superFire, _superAbilityTimer, _superFireCooldown);
                    
                            if (windUpgrade)
                            {
                                SetDualPower(4, _windList, _windCooldownList, "Wind", _wind, _windCapacity, _windTimer,
                                    _windCooldown, "SuperWind", _superWind, _superAbilityTimer, _superWindCooldown);
                        
                                if (lightningUpgrade)
                                {
                                    SetDualPower(5, _lightningList, _lightningCooldownList, "Lightning", _lightning,
                                        _lightningCapacity, _lightningTimer, _lightningCooldown, "SuperLightning",
                                        _superLightning, _superAbilityTimer, _superLightningCooldown);
                                }
                            }
                        }
                    }
                } 
                
                SetAbilityDictionary();
            }
        }

        private void SetUniquePower(int num, List<Ability.Ability> powerList, List<float> cooldownPowerList, string tag , GameObject power, int capacity, float timer, float cooldown)
        {

            powerList = new List<Ability.Ability>();

            //cooldownPowerList = new List<float>();
            
            powerList.Add(SetEachPower(tag, power, capacity, timer, cooldown));
            
            _abilities.Add(powerList);
            
            /*for (int i = 0; i < _shurikenCapacity; i++)
                {
                    _shurikenCooldownList.Add(_initialTime);
                }
                
            _allAbilitiesCooldowns.Add(num, _shurikenCooldownList);*/

        }

        private void SetDualPower(int num, List<Ability.Ability> powerList, List<float> cooldownPowerList, string tag , 
            GameObject power, int capacity, float timer, float cooldown, string superTag, GameObject superPower, float superTimer, float superCooldown)
        {
            powerList = new List<Ability.Ability>();

            //cooldownPowerList = new List<float>();
            
            powerList.Add(SetEachPower(tag, power, capacity, timer, cooldown));
            powerList.Add(SetEachPower(superTag, superPower, _superAbilityCapacity, superTimer, superCooldown));
            
            _abilities.Add(powerList);
                    
            /*for (int i = 0; i <= capacity; i++)
            {
                cooldownPowerList.Add(_initialTime);
            }

            _allAbilitiesCooldowns.Add(num, cooldownPowerList);*/
                    
            _maxPowers += 1;

        }

        private Ability.Ability SetEachPower(string tag , GameObject power, int capacity, float timer, float cooldown)
        {
            Ability.Ability abilityAux = power.GetComponent<Ability.Ability>();
            abilityAux.setTag(tag);
            abilityAux.setAbility(power);
            abilityAux.setSize(capacity);
            abilityAux.setTimer(timer);
            abilityAux.setCast(true);
            abilityAux.setCoolDown(cooldown);

            return abilityAux;
        }

        private void SetAbilityDictionary()
        {
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

            if (_maxPowers <= 0)
            {
                return;
            }
            _cooldownActiveAbility.ShowAbilities(_powerNum, _abilities[_powerNum][0].getSize());
        }

        private void ChoosePowerWithButton()
        {
            if (Input.GetKey(KeyCode.Alpha1) && rockUpgrade)
            {
                _powerNum = 1;
            }
            else if (Input.GetKey(KeyCode.Alpha2) && waterUpgrade)
            {
                _powerNum = 2;
            }
            else if (Input.GetKey(KeyCode.Alpha3) && fireUpgrade)
            {
                _powerNum = 3;
            }
            else if (Input.GetKey(KeyCode.Alpha4) && windUpgrade)
            {
                _powerNum = 4;
            }
            else if (Input.GetKey(KeyCode.Alpha5) && lightningUpgrade)
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
            if (_maxPowers <= 0)
            {
                return;
            }
            
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

            if (Input.GetMouseButton(0))
            {
                return;
            }
            _useSuperAbility = true;
            holdTime = _auxHoldTime;
        }
        
        private void UsePower(bool holded)
        {
            List<Ability.Ability> ability = new List<Ability.Ability>();
            
            int num = holded ? 1 : 0;
            
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

            CastAbility(tag, num);
        }
        
        private void CastAbility(string tag, int num)
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

                /*_actualTime = Time.timeSinceLevelLoad;

                _auxList = _allAbilitiesCooldowns[_powerNum];

                float auxDate = Time.timeSinceLevelLoad;
                
                if (num == 0)
                {
                    int auxValue = 0;

                    for (int i = 0; i < _auxList.Count - 1; i++)
                    {
                        if (_auxList[i] < auxDate)
                        {
                            auxDate = _auxList[i];
                            auxValue = i;
                        }
                    }
                    
                    _auxList[auxValue] = _actualTime;

                }
                else
                {
                    _auxList[_auxList.Count - 1] = _actualTime;
                }

                _auxSpecificAbilityList = _cooldownActiveAbility.UpdateCooldown(
                    _allAbilitiesCooldowns[_powerNum], _abilities[_powerNum][0].getSize(),
                    _abilities[_powerNum][0].getCooldown(), _abilities[_powerNum][1].getCooldown(), true);

                for (int i = 0; i < _auxSpecificAbilityList.Count; i++)
                {
                    _allAbilitiesCooldowns[_powerNum][i] = _auxSpecificAbilityList[i];
                }*/

                if (abilityComponent.getTimer() > 0 && !tag.Equals("SuperRock"))
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
                
                case Abilities.SHURIKEN:
                    shurikenUpgrade = true;
                    SetPowers();
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
            }
        }
        
        
        public void DeactivateAbility(Abilities ability)
        {
            switch (ability)
            {
                case Abilities.JUMP_UPGRADE:
                    jumpUpgrade = false;
                    break;
                    
                case Abilities.DOUBLE_JUMP_UPGRADE:
                    doubleJumpUpgrade = false;
                    break;
                
                case Abilities.SHURIKEN:
                    shurikenUpgrade = false;
                    SetPowers();
                    break;
                
                case Abilities.ROCK:
                    rockUpgrade = false;
                    SetPowers();
                    break;
                
                case Abilities.WATER:
                    waterUpgrade = false;
                    SetPowers();
                    break;
                
                case Abilities.FIRE:
                    fireUpgrade = false;
                    SetPowers();
                    break;
                
                case Abilities.WIND:
                    windUpgrade = false;
                    SetPowers();
                    break;
                
                case Abilities.LIGHTNING:
                    lightningUpgrade = false;
                    SetPowers();
                    break;
            }
        }
    }
}