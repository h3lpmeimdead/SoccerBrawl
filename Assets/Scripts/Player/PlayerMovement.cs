using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    enum PlayerStates { Idle, Running, Airborne}
    PlayerStates _state;
    bool _stateComplete;

    [Header("Reference")]
    [SerializeField] private PlayerMovementStats MoveStats;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;

    private Rigidbody2D _rb;

    //movement
    private Vector2 _moveVelocity;

    //collision check
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    //jump
    public float VerticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    //apex
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    //jump buffer
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    //coyote time
    private float _coyoteTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        JumpChecks();
        CountTimers();
        if(_stateComplete)
        {
            SelectState();
        }
        UpdateState();
    }

    private void FixedUpdate()
    {
        CollisionCheck();
        Jump();
        if (_isGrounded)
        {
            Move(MoveStats._groundAcceleration, MoveStats._groundDeceleration, InputManager._movement);
        }
        else Move(MoveStats._airAcceleration, MoveStats._airDeceleration, InputManager._movement);
    }

    void UpdateIdle()
    {

    }

    void UpdateRun()
    {

    }

    void UpdateAirborne()
    {

    }
    private void UpdateState()
    {
        switch (_state)
        {
            case PlayerStates.Idle:
                UpdateIdle();
                break;
            case PlayerStates.Running:
                UpdateRun();
                break;
            case PlayerStates.Airborne:
                UpdateAirborne();
                break;
        }
    }

    private void SelectState()
    {
        _stateComplete = false;
        if(_isGrounded)
        {
            //if()
        }
        else _state = PlayerStates.Airborne;
    }

    #region Movement
    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            Vector2 targetVelocity = Vector2.zero;
            targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats._maxWalkSpeed;
            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
        else if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
    }
    #endregion

    #region Jump
    private void JumpChecks()
    {
        //when we press the jump button 
        if (InputManager._jumpWasPressed)
        {
            _jumpBufferTimer = MoveStats._jumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }
        //when we release the jump button 
        if (InputManager._jumpWasReleased)
        {
            if (_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }
            if (_isJumping && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = MoveStats._timeForUpwardsCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }
        //initiate jump with jump buffering and coyote time
        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            InitiateJump(1);

            if(_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }
        //double jump
        else if(_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats._numberofJumpsAllowed)
        {
            _isFastFalling = false;
            InitiateJump(1);
        }
        //air jump after coyote time lapsed
        else if(_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MoveStats._numberofJumpsAllowed - 1)
        {
            InitiateJump(2);
            _isFastFalling = false;
        }
        //landed
        if((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int numberofJumpsUsed)
    {
        if(!_isJumping)
        {
            _isJumping = true;
        }

        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberofJumpsUsed;
        VerticalVelocity = MoveStats.InitialJumpVelocity;
    }

    private void Jump()
    {
        //apply gravity while jumping 
        if(_isJumping)
        {
            //check for head bump
            if(_bumpedHead)
            {
                _isFastFalling = true;
            }

            //gravity on ascending
            if(VerticalVelocity >= 0f)
            {
                //apex controls
                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if(_apexPoint > MoveStats._apexThreshHold)
                {
                    if(!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if(_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < MoveStats._apexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else VerticalVelocity = -0.01f;
                    }
                }

                //gravity on ascending but not past apex threshold
                else
                {
                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }

            //gravity on descending
            else if (!_isFastFalling)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats._gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (VerticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }
        
        //jump cut
        if(_isFastFalling)
        {
            if(_fastFallTime >= MoveStats._timeForUpwardsCancel)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats._gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if(_fastFallTime < MoveStats._timeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats._timeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime;
        }
        //normal gravity while falling
        if(!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }
            
            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
        }

        //clamp fall speed
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats._maxFallSpeed, 50f);
        _rb.velocity = new Vector2(_rb.velocity.x, VerticalVelocity);
    }

    #endregion
    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * MoveStats._headWidth, MoveStats._headDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats._headDetectionRayLength, MoveStats._groundLayer);
        if (_headHit.collider != null)
        {
            _bumpedHead = true;
        }
        else _bumpedHead = false;

        #region Debug Visualization
        if(MoveStats._debugShowHeadBumpBox)
        {
            float headWitdth = MoveStats._headWidth;

            Color rayColor;
            if (_bumpedHead)
            {
                rayColor = Color.green;
            }
            else rayColor = Color.red;

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWitdth, boxCastOrigin.y ), Vector2.up * MoveStats._headDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWitdth, boxCastOrigin.y), Vector2.up * MoveStats._headDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWitdth, boxCastOrigin.y + MoveStats._headDetectionRayLength), Vector2.right * boxCastSize.x * headWitdth, rayColor);
        }

        #endregion
    }

    #region Collision check
    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats._groundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats._groundDetectionRayLength, MoveStats._groundLayer);
        if (_groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else _isGrounded = false;
        #region Debug Visualisation
        if (MoveStats._debugShowIsGroundedBox)
        {
            Color rayColor;
            if (_isGrounded)
            {
                rayColor = Color.green;
            }
            else rayColor = Color.red;

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats._groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats._groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats._groundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);

        }
        #endregion
    }

    private void CollisionCheck()
    {
        IsGrounded();
    }
    #endregion

    #region Timer
    
    private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;
        
        if(!_isGrounded)
        {
            _coyoteTimer -= Time.deltaTime;
        }
        else _coyoteTimer = MoveStats._jumpCoyoteTime;
    }

    #endregion
}
