using CameraController;
using UnityEngine;

namespace Character
{
    public class CharacterScript : MonoBehaviour
{
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
    
    private Vector3 _objectPos;
    private Vector3 _posCamera;

    private GameObject _rocksList;
    private GameObject _waterList;
    private GameObject _fireList;
    private GameObject _windList;
    private GameObject _lightningList;
    
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
    
    private int powerNum = 1;
    private int rockCapacity = 2;
    private int waterCapacity = 1;
    private int fireCapacity = 1;
    private int windCapacity = 1;
    private int lightningCapacity = 1;
    private int speed = 50;


    void Start()
    {
        _maximumZoomOut = Camera.main.orthographicSize;
        _posCamera = Camera.main.transform.position;
        _rb2D = GetComponent<Rigidbody2D>();
        setPowers();
        jumpUpgrade = true;
        //doubleJumpUpgrade = true;
    }

    void Update()
    {

        Jump();


        //powerNum = choosePower(powerNum);
        if (!Input.GetMouseButton(1))
        {
            Zoom();
        }
        else
        {
            powerNum = choosePower(powerNum);
            
            if (Input.GetMouseButtonUp(0))
            {
                UsePower(powerNum);
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

    private void setPowers()
    {
        if (true)
        {
            _rocksList = new GameObject();
            _rocksList.name = "RocksList";

            if (_waterUpgrade)
            {
                _waterList = new GameObject();
                _waterList.name = "WaterList";

                if (_fireUpgrade)
                {
                    _fireList = new GameObject();
                    _fireList.name = "FireList";

                    if (_windUpgrade)
                    {
                        _windList = new GameObject();
                        _windList.name = "WindList";

                        if (_lightningUpgrade)
                        {
                            _lightningList = new GameObject();
                            _lightningList.name = "LightningList";
                        }
                    }
                }
            }
        }
    }

    private int choosePower(int powerNum)
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            powerNum += 1;
            if (powerNum == 6)
            {
                powerNum = 1;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            powerNum -= 1;
            if (powerNum == 0)
            {
                powerNum = 5;
            }
        }

        return powerNum;
    }
    
    private void UsePower(int powerNum)
    {
        switch (powerNum)
        {
            case 1:
                if (_rocksList.transform.childCount > rockCapacity - 1)
                {
                    Destroy(_rocksList.transform.GetChild(0).gameObject);
                }
                _objectPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _objectPos.z = 0f;
                Instantiate(rock, _objectPos, Quaternion.identity, _rocksList.transform);
                break;
                
            case 2:
                _objectPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _objectPos.z = 0f;
                Instantiate(water, _objectPos, Quaternion.identity);
                break;
                
            case 3:
                _objectPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _objectPos.z = 0f;
                Instantiate(fire, _objectPos, Quaternion.identity);
                break;
                
            case 4:
                _objectPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _objectPos.z = 0f;
                Instantiate(wind, _objectPos, Quaternion.identity);
                break;
                
            case 5:
                _objectPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _objectPos.z = 0f;
                Instantiate(lightning, _objectPos, Quaternion.identity);
                break;
                    
        }

    }

    private void Zoom()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        if (zoom > 0f)
        {
            if (Camera.main.orthographicSize > _maximumZoomIn)
            {
                Camera.main.orthographicSize -= 0.1f;
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, _posCamera.z), Time.deltaTime * speed);
                //Camera.main.GetComponent<CameraMoves>().enabled = true;
            }
        } 
        else if (zoom < 0f)
        {
            if (Camera.main.orthographicSize < _maximumZoomOut)
            {
                Camera.main.orthographicSize += 0.1f;
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,  _posCamera, Time.deltaTime * speed);
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

