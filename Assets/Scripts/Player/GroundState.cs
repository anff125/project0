using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundState : StateTemplate
{
    private bool _groundedPlayer;
    private readonly float _playerSpeed;
    private readonly float _gravityValue;
    private readonly PlayerInput _playerInput;

    private static readonly int Speed = Animator.StringToHash("characterSpeed");
    private static readonly int IsMove = Animator.StringToHash("isMove");
    private static readonly int IsAiming = Animator.StringToHash("isAiming");

    private readonly GameObject _camera;
    private readonly GameObject _aimCamera;
    private bool _jumpPressed;
    private bool _attackPressed;
    private bool _isAiming;

    public GroundState(GameObject player, Animator animator, CharacterController controller,
        StateMachine stateMachine) : base(player, animator, controller,
        stateMachine)
    {
        this._playerSpeed = stateMachine.moveSpeed;
        this._gravityValue = stateMachine.gravityValue;
        _playerInput = InputManager.PlayerInput;
        _camera = stateMachine._camera;
        _aimCamera = stateMachine._aimCamera;
        if (_playerInput == null)
        {
            Debug.Log("Player input is null");
        }

        _playerInput.actions["Jump"].performed += ctx => _jumpPressed = true;
        _playerInput.actions["Attack"].performed += ctx => _attackPressed = true;
        _playerInput.actions["Aim"].started += ctx =>
        {
            if (StateMachine.CurrentWeaponIndex == 2)
            {
                _isAiming = true;
                Animator.SetLayerWeight(1, 1);
                Animator.SetBool(IsAiming, true);
                Debug.Log("Aim started");

                _aimCamera.SetActive(true);
                _camera.SetActive(false);
            }
        };

        _playerInput.actions["Aim"].canceled += ctx =>
        {
            _isAiming = false;
            Animator.SetLayerWeight(1, 0);
            Animator.SetBool(IsAiming, false);
            Debug.Log("Aim canceled");
            _aimCamera.SetActive(false);
            _camera.SetActive(true);
        };
    }

    public override void Enter()
    {
        base.Enter();
        Animator.SetBool(IsMove, true);
        _jumpPressed = false;
        _attackPressed = false;
    }

    public override void Exit()
    {
        base.Exit();
        Animator.SetBool(IsMove, false);
    }

    public override void Update()
    {
        HandleMovement();
        switch (_groundedPlayer)
        {
            case true when _jumpPressed:
                StateMachine.ChangeState(StateMachine.JumpState);
                break;
            case true when _attackPressed:
                switch (StateMachine.CurrentWeaponIndex)
                {
                    case 1:
                        StateMachine.ChangeState(StateMachine.SwordAttackState);
                        break;
                    case 2:
                        if (_isAiming)
                        {
                            StateMachine.ChangeState(StateMachine.ShootingArrowState);
                        }

                        break;
                    default:
                        StateMachine.ChangeState(StateMachine.HandAttackState);
                        break;
                }

                break;
            case false:
                StateMachine.ChangeState(StateMachine.AirState);
                break;
        }
    }

    public override void FixedUpdate()
    {
        ApplyGravity();
        HandleGroundedState();
    }

    private void HandleGroundedState()
    {
        _groundedPlayer = Controller.isGrounded;
        if (!_groundedPlayer || !(StateMachine.PlayerVelocity.y < 0)) return;
        var vector3 = StateMachine.PlayerVelocity;
        vector3.y = 0f;
        StateMachine.PlayerVelocity = vector3;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void HandleMovement()
    {
        var moveInput = _playerInput.actions["Move"].ReadValue<Vector2>();
        var currentCamera = _isAiming ? _aimCamera : _camera;
        if (!currentCamera)
        {
            Debug.Log("No camera found");
            return;
        }

        var cameraForward = currentCamera.transform.forward;
        var cameraRight = currentCamera.transform.right;

        cameraForward.y = 0; // Ignore camera pitch to keep movement horizontal
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Transform moveInput from local space to world space relative to the camera's orientation
        var move = (cameraForward * moveInput.y + cameraRight * moveInput.x) * _playerSpeed;

        Controller.Move(move * Time.deltaTime);

        if (move != Vector3.zero)
        {
            if (_isAiming)
            {
                Player.transform.forward = cameraForward;
            }
            else
            {
                Player.transform.forward = move;
            }
        }

        Animator.SetFloat(Speed, moveInput.magnitude);
    }

    private void ApplyGravity()
    {
        var vector3 = StateMachine.PlayerVelocity;
        vector3.y += _gravityValue * Time.deltaTime;
        StateMachine.PlayerVelocity = vector3;
        Controller.Move(StateMachine.PlayerVelocity * Time.deltaTime);
    }
}