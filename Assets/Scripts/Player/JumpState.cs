using UnityEngine;

public class JumpState : StateTemplate
{
    private static readonly int IsJump = Animator.StringToHash("isJump");
    private bool _groundedPlayer;
    private readonly float _gravityValue;
    private readonly float _jumpHeight;

    public JumpState(GameObject player, Animator animator, CharacterController controller,
        StateMachine stateMachine) : base(player, animator, controller,
        stateMachine)
    {
        _gravityValue = StateMachine.gravityValue;
        _jumpHeight = StateMachine.jumpForce;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Enter()
    {
        base.Enter();
        Animator.SetBool(IsJump, true);
        SetStateChangeCooldown(0.5f);
        HandleJump();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Update()
    {
        base.Update();
        ApplyGravity();
        if (Controller.isGrounded)
            StateMachine.ChangeState(StateMachine.GroundState);
        else
            StateMachine.ChangeState(StateMachine.AirState);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Exit()
    {
        base.Exit();
        Animator.SetBool(IsJump, false);
    }

    private void ApplyGravity()
    {
        var vector3 = StateMachine.PlayerVelocity;
        vector3.y += _gravityValue * Time.deltaTime;
        StateMachine.PlayerVelocity = vector3;
        Controller.Move(StateMachine.PlayerVelocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        var vector3 = StateMachine.PlayerVelocity;
        vector3.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        StateMachine.PlayerVelocity = vector3;
    }
}