using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    private LayerMask _layerMasks;
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
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }
}
