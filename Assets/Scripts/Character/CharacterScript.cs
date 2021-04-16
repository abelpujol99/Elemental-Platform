using System;
using System.Collections;
using System.Collections.Generic;
using Ability.Abilities;
using System.Reflection;
using System.Security.Cryptography;
using CameraController;
using Ability;
using UnityEngine;

namespace Character
{
    public class CharacterScript : MonoBehaviour
    {
        /*[System.Serializable]
        public class AbilityParameters
        {
            private string tag;
            private GameObject ability;
            private float size;
            private float timer;
            private bool cast;
            private float cooldown;

            public AbilityParameters(string tag, GameObject ability, float size, float timer, bool cast, float cooldown)
            {
                this.tag = tag;
                this.ability = ability;
                this.size = size;
                this.timer = timer;
                this.cast = cast;
                this.cooldown = cooldown;
            }


            public string getTag()
            {
                return tag;
            }

            public GameObject getAbility()
            {
                return ability;
            }

            public float getSize()
            {
                return size;
            }

            public float getTimer()
            {
                return timer;
            }

            public bool isCast()
            {
                return cast;
            }

            public float getCooldown()
            {
                return cooldown;
            }

            public void setSize(int size)
            {
                this.size = size;
            }

            public void setTimer(float timer)
            {
                this.timer = timer;
            }

            public void setCast(bool cast)
            {
                this.cast = cast;
            }

            public void setCoolDown(float cooldown)
            {
                this.cooldown = cooldown;
            }
        }*/
        
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

        [SerializeField] private GameObject rock;
        [SerializeField] private GameObject water;
        [SerializeField] private GameObject fire;
        [SerializeField] private GameObject wind;
        [SerializeField] private GameObject lightning;
        [SerializeField] private GameObject superRock;
        [SerializeField] private GameObject superWater;
        [SerializeField] private GameObject superFire;
        [SerializeField] private GameObject superWind;
        [SerializeField] private GameObject superLightning;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
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
                
                _rocksList.Add(new Rock("Rock", rock, _rockCapacity, 0f, true, 1f));
                _rocksList.Add(new Rock("SuperRock", superRock, _superAbilityCapacity, _superAbilityTimer, true, 5f));
                
                _abilities.Add(_rocksList);
                _maxPowers += 1;

                if (waterUpgrade)
                {
                    _waterList = new List<Ability.Ability>();
                    
                    _waterList.Add(new Water("Water", water, _waterCapacity, 1f, true, 2f));
                    _waterList.Add(new Water("SuperWater", superWater, _superAbilityCapacity, _superAbilityTimer, true, 4f));
                    
                    _abilities.Add(_waterList);
                    _maxPowers += 1;

                    if (fireUpgrade)
                    {
                        _fireList = new List<Ability.Ability>();
                        
                        _fireList.Add(new Fire("Fire", fire, _fireCapacity, 2f, true, 3f));
                        _fireList.Add(new Fire("SuperFire", superFire, _superAbilityCapacity, _superAbilityTimer, true, 5f));
                        
                        _abilities.Add(_fireList);
                        _maxPowers += 1;

                        if (windUpgrade)
                        {
                            _windList = new List<Ability.Ability>();
                            
                            _windList.Add(new Wind("Wind", wind, _windCapacity, 3f, true, 1f));
                            _windList.Add(new Wind("SuperWind", superWind, _superAbilityCapacity, _superAbilityTimer, true, 4.5f));
                            
                            _abilities.Add(_windList);
                            _maxPowers += 1;

                            if (lightningUpgrade)
                            {
                                _lightningList = new List<Ability.Ability>();

                                _lightningList.Add(new Lightning("Lightning", lightning, _lightningCapacity, 0.5f, true, 2.5f));
                                _lightningList.Add(new Lightning("SuperLightning", superLightning, _superAbilityCapacity, _superAbilityTimer, true, 4f));
                                
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

            CastAbility(ability, tag);
        }
        private void CastAbility(List<Ability.Ability> ability, string tag)
        {
            GameObject abilityToSpawn;
            
            _abilityPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _abilityPos.z = 0f;
            
            float angle = Mathf.Atan2(_abilityPos.y - transform.position.y, _abilityPos.x - transform.position.x) * Mathf.Rad2Deg;
            float direction;
            
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (transform.position.x < _abilityPos.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            abilityToSpawn = spawnAbility(tag);

            /*if (_powerNum == 0)
            {
                abilityToSpawn.GetComponent<Rock>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
            }
            else if (_powerNum == 1)
            {
                abilityToSpawn.GetComponent<Water>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
            } 
            else if (_powerNum == 2)
            {
                abilityToSpawn.GetComponent<Fire>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
            } 
            else if (_powerNum == 3)
            {
                abilityToSpawn.GetComponent<Wind>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
            }
            else if (_powerNum == 4)
            {
                abilityToSpawn.GetComponent<Lightning>().abilityUtility(abilityToSpawn, _abilityPos, gameObject);
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

        /*private IEnumerator AbilityDisappear(float timer, GameObject abilityToSpawn)
        {
            yield return new WaitForSeconds(timer);
            abilityToSpawn.SetActive(false);
        }

        private IEnumerator AbilityCooldown(float cooldown, GameObject abilityToSpawn)
        {
            yield return new WaitForSeconds(cooldown);
            abilityToSpawn.GetComponent<Water>();
        }*/

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