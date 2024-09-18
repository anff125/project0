using UnityEngine;
using UnityEngine.PlayerLoop;

public class AirState : StateTemplate
{
    private readonly float _gravityValue;
    private bool _groundedPlayer;

    public AirState(GameObject player, Animator animator, CharacterController controller,
        StateMachine stateMachine) : base(player, animator, controller,
        stateMachine)
    {
        _gravityValue = stateMachine.gravityValue;
    }

    private static readonly int IsAir = Animator.StringToHash("isAir");

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Enter()
    {
        base.Enter();
        Animator.SetBool(IsAir, true);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Update()
    {
        base.Update();
        
        if (_groundedPlayer)
        {
            StateMachine.ChangeState(StateMachine.GroundState);
        }
    }

    public override void FixedUpdate()
    {
        ApplyGravity();
        _groundedPlayer = Controller.isGrounded;
    }

    public override void Exit()
    {
        base.Exit();
        Animator.SetBool(IsAir, false);
    }

    private void ApplyGravity()
    {
        var vector3 = StateMachine.PlayerVelocity;
        vector3.y += _gravityValue * Time.deltaTime;
        StateMachine.PlayerVelocity = vector3;
        Controller.Move(StateMachine.PlayerVelocity * Time.deltaTime);
    }
}