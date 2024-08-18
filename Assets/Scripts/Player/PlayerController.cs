using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : InputHandler
{
    #region Player Controls and Settings
    [SerializeField, Range(0.0f, 360.0f)]
    private float _slopeMax;
    [SerializeField, Range(0.0f, 10.0f)]
    private float _movementSpeed = 0.0f;
    [SerializeField, Range(0.0f, 10.0f)]
    private float _crouchSpeed = 0.0f;
    [SerializeField, Range(0.0f, 10.0f)]
    private float _jumpForce = 0.5f;
    [SerializeField]
    private float _groundRay = 0.5f;
    [SerializeField]
    private LayerMask _groundLayerMask;
    [SerializeField]
    private LayerMask _slopeLayerMask;
    [SerializeField]
    #endregion

    private Vector2 _sideBoxSize = new Vector2(0.2f, 1.3f);
    private float _sideBoxDistance = 0.5f;
    private float _bottomGroundRadius = 0.1f;
    private float _bottomGroundDistnace = 1.1f;
    private Vector2 _topHeadBoxSize = new Vector2(0.4f, 1.5f);
    private float _topHeadBoxDistance = 0.5f;
    private float _topHeadRadius = 0.5f;
    private float _topHeadDistance = 0.8f;
    private float _forceAmount = 100.0f;
    private float _jumpDelayTime = 0.4f;
    private bool _allowInput = false;
    private bool _allowShoot = false;
    private float _inputDelayTime = 0.8f;
    private float _shootDelayTime = 0.05f;
    private CapsuleCollider2D _playerCapsule;
    private float _playerHeight;
    private Rigidbody2D _playerRb;
    private Coroutine _jumpCoroutine;
    private Coroutine _inputCoroutine;
    private Coroutine _shootCoroutine;
    private Vector2 _slopePerpendicular;
    private Direction _direction;

    //Player finite state machine.
    private FullActionStatus _fullStatus;
    private BottomActionStatus _botStatus;
    private TopActionStatus _topStatus;

    // Start is called before the first frame update
    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerCapsule = GetComponent<CapsuleCollider2D>();
        _playerHeight = _playerCapsule.size.y / 2;
        _fullStatus = transform.GetComponentInChildren<FullActionStatus>(true);
        _botStatus = transform.GetComponentInChildren<BottomActionStatus>(true);
        _topStatus = transform.GetComponentInChildren<TopActionStatus>(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        bool isLeftPressed = !RaycastHelper.CheckBoxSide(transform.position, Vector2.left, _sideBoxDistance, _sideBoxSize, _groundLayerMask) && Input.GetKey(MoveLeft);
        bool isRightPressed = !RaycastHelper.CheckBoxSide(transform.position, Vector2.right, _sideBoxDistance, _sideBoxSize, _groundLayerMask) && Input.GetKey(MoveRight);
        bool isGround = RaycastHelper.CheckCircleSide(transform.position, Vector2.down, _bottomGroundRadius, _bottomGroundDistnace, _groundLayerMask);
        bool isSlope = CheckSlope(-Vector2.up, _bottomGroundRadius, _bottomGroundDistnace, _slopeLayerMask);
        bool isCrouch = Input.GetKey(MoveDown);
        bool isLookUp = Input.GetKey(MoveUp);
        bool isShoot = Input.GetKeyDown(ShootKey) && !_allowShoot; // if the player has pressed 
        bool isAnyDirectionKeyPressed = isLeftPressed || isRightPressed;
        bool isAnyDirectionKeyNotPressed = !isLeftPressed || !isRightPressed;
        float movementSpeed = (isCrouch && isGround) ? _crouchSpeed : _movementSpeed;

        if (isAnyDirectionKeyPressed && !isSlope && !_allowInput)
        {
            Vector2 movePosition = new Vector2(horizontal * _forceAmount * movementSpeed, 0.0f);
            _playerRb.gravityScale = 2.0f;
            _playerRb.velocity = new Vector2(movePosition.x * Time.fixedDeltaTime, _playerRb.velocity.y);
        }
        else if (isAnyDirectionKeyPressed && isSlope && !_allowInput)
        {
            Vector2 slopeDirection = new Vector2(horizontal * _forceAmount * movementSpeed * -_slopePerpendicular.x, -_slopePerpendicular.y * _movementSpeed);
            Vector2 movePosition = new Vector2(slopeDirection.x * Time.fixedDeltaTime, _playerRb.velocity.y);
            _playerRb.gravityScale = 0.0f;
            _playerRb.velocity = movePosition;
        }
        else if (isAnyDirectionKeyNotPressed && isSlope)
        {
            _playerRb.velocity = Vector2.zero;
        }

        if (!isCrouch && _allowInput && (!isLookUp || isLookUp))
        {
            StopCoroutine(nameof(DelayInput));
            _inputCoroutine = null;
            _allowInput = false;
        }

        if (Input.GetKeyDown(JumpKey) && isGround && _jumpCoroutine == null)
        {
            _jumpCoroutine = StartCoroutine(nameof(DelayJump));
            Vector2 jumpAmount = Vector2.up * _jumpForce * _forceAmount * Time.fixedDeltaTime;
            _playerRb.velocity = jumpAmount;
            _playerRb.gravityScale = 2.0f;
            _playerCapsule.isTrigger = true;
        }



        //Player Animation Status
        if (isLeftPressed)
        {
            _direction = Direction.Left;
            //Full Animation
            SpriteHelper.ChangeSpritePosition(_fullStatus.gameObject, true, new Vector2(0.1f, -1.0f));
            //Top animation
            SpriteHelper.ChangeSpritePosition(_topStatus.gameObject, true, new Vector2(0.1f, 0.9f));
            //Bottom animation
            SpriteHelper.ChangeSpritePosition(_botStatus.gameObject, true, new Vector2(0.1f, 0.9f));
        }
        else if (isRightPressed)
        {
            _direction = Direction.Right;
            //Full Animation
            SpriteHelper.ChangeSpritePosition(_fullStatus.gameObject, false, new Vector2(-0.1f, -1.0f));
            //Top animation
            SpriteHelper.ChangeSpritePosition(_topStatus.gameObject, false, new Vector2(-0.1f, 0.9f));
            //Bottom animation
            SpriteHelper.ChangeSpritePosition(_botStatus.gameObject, false, new Vector2(-0.1f, 0.9f));
        }
        _fullStatus.gameObject.SetActive(isCrouch && isGround);
        _botStatus.gameObject.SetActive(!isCrouch || (isCrouch && !isGround));
        _topStatus.gameObject.SetActive(!isCrouch || (isCrouch && !isGround));
        //Player Movement, Jump, Crouch, Shoot
        if (isAnyDirectionKeyPressed && isGround && !isCrouch)
        {
            if (isLookUp && !isShoot)
            {
                _topStatus.PlayerAction = Action.TopAimUp;
            }
            else if (isLookUp && isShoot)
            {
                Shoot(Direction.Up);
                _topStatus.PlayerAction = Action.TopShootUp;
            }
            else if (!isLookUp && isShoot)
            {
                Shoot(_direction);
                _topStatus.PlayerAction = Action.TopShootRight;
            }
            else
            {
                _topStatus.PlayerAction = Action.TopMoveRight;
            }
            _botStatus.PlayerAction = Action.BotMoveRight;
        }
        else if (isAnyDirectionKeyPressed && !isGround)
        {
            if (isLookUp && !isShoot && !isCrouch)
            {
                _topStatus.PlayerAction = Action.TopAimUp;
            }
            else if (isLookUp && isShoot && !isCrouch)
            {
                Shoot(Direction.Up);
                _topStatus.PlayerAction = Action.TopShootUp;
            }
            else if (!isLookUp && isShoot && !isCrouch)
            {
                Shoot(_direction);
                _topStatus.PlayerAction = Action.TopShootRight;
            }
            else if (!isLookUp && isCrouch && isShoot)
            {
                Shoot(Direction.Down);
                _topStatus.PlayerAction = Action.TopShootDown;
            }
            else
            {
                _topStatus.PlayerAction = Action.TopJumpRight;
            }
            _botStatus.PlayerAction = Action.BotJumpRight;
        }
        else if (isAnyDirectionKeyNotPressed && !isGround)
        {
            if (isCrouch && !isLookUp && !isShoot)
            {
                _topStatus.PlayerAction = Action.TopAimDown;
            }
            else if (isLookUp && !isCrouch && !isShoot)
            {
                _topStatus.PlayerAction = Action.TopAimUp;
            }
            else if (isLookUp && !isCrouch && isShoot)
            {
                Shoot(Direction.Up);
                _topStatus.PlayerAction = Action.TopShootUp;
            }
            else if (!isLookUp && isCrouch && isShoot)
            {
                Shoot(Direction.Down);
                _topStatus.PlayerAction = Action.TopShootDown;
            }
            else
            {
                _topStatus.PlayerAction = Action.TopIdle;
            }
            _botStatus.PlayerAction = Action.BotJumpIdle;
        }
        else if (isAnyDirectionKeyNotPressed && !isGround)
        {

            _topStatus.PlayerAction = isCrouch ? Action.TopAimDown : Action.TopIdle;
            _botStatus.PlayerAction = Action.BotJumpIdle;
        }
        else if (isAnyDirectionKeyPressed && isCrouch && isGround)
        {
            if (!isLookUp && isShoot)
            {
                if (_inputCoroutine == null) _inputCoroutine = StartCoroutine(nameof(DelayInput));
                Shoot(_direction);
                _fullStatus.PlayerAction = Action.FullCrouchShoot;
            }
            else
            {
                _fullStatus.PlayerAction = Action.FullCrouchMoveRight;
            }
        }
        else if (isCrouch && isGround)
        {
            //calltime occurs
            if (!isLookUp && isShoot)
            {
                if (_inputCoroutine == null) _inputCoroutine = StartCoroutine(nameof(DelayInput));
                Shoot(_direction);
                _fullStatus.PlayerAction = Action.FullCrouchShoot;
            }
            else
            {
                _fullStatus.PlayerAction = Action.FullCrouch;
            }
        }
        else if (!isCrouch && !isGround)
        {
            _topStatus.PlayerAction = (!isLookUp && isShoot) ? Action.TopShootRight : Action.TopIdle;
            _botStatus.PlayerAction = Action.BotJumpIdle;
        }
        else
        {
            if (isLookUp && !isShoot)
            {
                _topStatus.PlayerAction = Action.TopAimUp;
            }
            else if (isLookUp && isShoot)
            {
                Shoot(Direction.Up);
                _topStatus.PlayerAction = Action.TopShootUp;
            }
            else if (!isLookUp && isShoot)
            {
                Shoot(_direction);
                _topStatus.PlayerAction = Action.TopShootRight;
            }
            else
            {
                _topStatus.PlayerAction = Action.TopIdle;

            }
            _botStatus.PlayerAction = Action.BotIdle;
        }

    }

    private IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(_jumpDelayTime);
        StopCoroutine(_jumpCoroutine);
        _jumpCoroutine = null;
        _playerCapsule.isTrigger = false;
    }
    private IEnumerator DelayInput()
    {
        _allowInput = true;
        yield return new WaitForSeconds(_inputDelayTime);
        StopCoroutine(_inputCoroutine);
        _inputCoroutine = null;
        _allowInput = false;
    }
    private IEnumerator DelayShoot()
    {
        _allowShoot = true;
        yield return new WaitForSeconds(_shootDelayTime);
        StopCoroutine(_shootCoroutine);
        _shootCoroutine = null;
        _allowShoot = false;
    }
    private void Shoot(Direction direction)
    {
        if (_shootCoroutine == null)
        {
            GameObject bullet = ObjectPool.Instance.GetObjectPool();
            if (bullet != null)
            {
                Projectile projectile = bullet.GetComponent<Projectile>();
                projectile.BulletDirection = direction;
                projectile.BulletActiveTime = 5.0f;
                projectile.transform.position = GetTransformDirection(transform.position, direction);
                projectile.gameObject.SetActive(true);
            }
            _shootCoroutine = StartCoroutine(nameof(DelayShoot));
        }
    }
    private Vector3 GetTransformDirection(Vector3 origin, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                {
                    return origin += new Vector3(1.0f, 1.5f, 0.0f);
                }
            case Direction.Down:
                {
                    return origin += new Vector3(1.0f, 1.5f, 0.0f);
                }
            case Direction.Left:
                {
                    return origin += new Vector3(-1.3f, 1.6f, 0.0f);
                }
            case Direction.Right:
                {
                    return origin += new Vector3(1.3f, 1.6f, 0.0f);
                }
        }
        return Vector3.zero;
    }
    private bool CheckSlope(Vector2 direction, float radius, float distance, LayerMask _layerMask)
    {
        RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, radius, direction, distance, _layerMask);
        if (hitInfo.collider != null)
        {
            _slopePerpendicular = Vector2.Perpendicular(hitInfo.normal).normalized;
            float angle = Vector2.Angle(hitInfo.normal, direction);
            Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red);
            Debug.DrawRay(hitInfo.point, _slopePerpendicular, Color.cyan);
            return angle != 0 && angle < _slopeMax;
        }
        _slopePerpendicular = Vector2.zero;
        return false;
    }
    private void OnDrawGizmos()
    {
        DrawHelper.SetTransform(transform);
        DrawHelper.DrawRaySphere(Vector2.up, _topHeadDistance, _topHeadRadius);
        DrawHelper.DrawRaySphere(Vector2.down, _bottomGroundDistnace, _bottomGroundRadius);
        DrawHelper.DrawRayBox(Vector2.right, _sideBoxDistance, _sideBoxSize);
        DrawHelper.DrawRayBox(Vector2.left, _sideBoxDistance, _sideBoxSize);
        DrawHelper.DrawyRayLine(Vector2.down, _playerHeight + _groundRay);
        DrawHelper.DrawCapsule(_playerHeight, 0.5f);
    }
}
