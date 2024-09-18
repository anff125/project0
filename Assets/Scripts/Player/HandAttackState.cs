using UnityEngine;

public class HandAttackState : StateTemplate
{
    private static readonly int IsHandAttack = Animator.StringToHash("isHandAttack");

    public HandAttackState(GameObject player, Animator animator, CharacterController controller,
        StateMachine stateMachine) : base(player, animator, controller,
        stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Animator.SetBool(IsHandAttack, true);
        SetStateChangeCooldown(0.5f);
    }

    public override void Update()
    {
        base.Update();
        if (CanExit) StateMachine.ChangeState(StateMachine.GroundState);
    }

    public override void Exit()
    {
        base.Exit();
        Animator.SetBool(IsHandAttack, false);
    }
}