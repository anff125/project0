using System;
using Card;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public class StateMachine : MonoBehaviour
{
    public StateTemplate _currentState;
    public StateTemplate _previousState;
    private Animator _animator;
    private GameObject _player;
    private CharacterController _controller;
    private PlayerInput _playerInput;
    public GameObject _camera;
    public GameObject _aimCamera;
    public static ObjectPool arrowPool;

    #region movement

    [FormerlySerializedAs("Move Speed")] [SerializeField]
    public float moveSpeed;

    [FormerlySerializedAs("Jump Force")] [SerializeField]
    public float jumpForce;

    [FormerlySerializedAs("Gravity Value")] [SerializeField]
    public float gravityValue;

    [SerializeField] private Vector3 playerVelocity;

    public Vector3 PlayerVelocity
    {
        get => playerVelocity;
        set => playerVelocity = value;
    }

    #endregion

    #region states

    public GroundState GroundState;
    public AirState AirState;
    public JumpState JumpState;
    public HandAttackState HandAttackState;
    public SwordAttackState SwordAttackState;
    public ShootingArrowState ShootingArrowState;
    public DashState DashState;
    public CycloneState CycloneState;

    #endregion

    public int CurrentWeaponIndex { get; private set; } = 0;

    public Transform leftHand;

    private Vector2 _movementInput;
    private bool _isMoving;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = gameObject;
        _controller = GetComponent<CharacterController>();
        arrowPool = GetComponent<ObjectPool>();

        GroundState = new GroundState(_player, _animator, _controller, this);
        AirState = new AirState(_player, _animator, _controller, this);
        JumpState = new JumpState(_player, _animator, _controller, this);
        HandAttackState = new HandAttackState(_player, _animator, _controller, this);
        SwordAttackState = new SwordAttackState(_player, _animator, _controller, this);
        ShootingArrowState = new ShootingArrowState(_player, _animator, _controller, this);
        DashState = new DashState(_player, _animator, _controller, this);
        CycloneState = new CycloneState(_player, _animator, _controller, this);
    }

    private int _currentWeaponDurability;

    public int CurrentWeaponDurability
    {
        get => _currentWeaponDurability;
        set
        {
            _currentWeaponDurability = value;
            if (_currentWeaponDurability <= 0)
            {
                WeaponManager.Instance.DeactivateWeapon();
            }
        }
    }

    public void WearWeapon(int index)
    {
        CurrentWeaponIndex = index;

        var weaponCard = CardLaboratory.Instance.cardlist[index] as CardType_Weapon;
        if (weaponCard != null)
        {
            CurrentWeaponDurability = weaponCard.Durability;
        }
        else
        {
            Debug.Log("Card is not a weapon card!");
        }
    }

    private void Start()
    {
        ChangeState(GroundState);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _currentState.Update();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    public void ChangeState(StateTemplate newStateTemplate)
    {
        if (_currentState is { CanExit: false }) return;
        _previousState = _currentState;

        _currentState?.Exit();
        _currentState = newStateTemplate;
        _currentState.Enter();
    }
}