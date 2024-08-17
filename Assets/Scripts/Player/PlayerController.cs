using System.Collections;
using System.Collections.Generic;
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
        _fullStatus = transform.GetComponentInChildren<FullActionStatus>(true);
        _botStatus = transform.GetComponentInChildren<BottomActionStatus>(true);
        _topStatus = transform.GetComponentInChildren<TopActionStatus>(true);
        _playerHeight = _playerCapsule.size.y / 2;
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
        Debug.Log(direction.ToString());
        if (_shootCoroutine == null)
        {
            GameObject bullet = ObjectPool.Instance.GetObjectPool();
            _shootCoroutine = StartCoroutine(nameof(DelayShoot));
        }
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
}
