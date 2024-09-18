using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackState : StateTemplate
{
    private static readonly int IsSwordAttack = Animator.StringToHash("isSwordAttack");

    public SwordAttackState(GameObject player, Animator animator, CharacterController controller,
        StateMachine stateMachine) : base(player, animator, controller, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (StateMachine.CurrentWeaponIndex != 1)
        {
            Debug.Log("Weapon is not sword");
            // stop game
            throw new System.NotImplementedException();
        }
        
        Animator.SetBool(IsSwordAttack, true);
        SetStateChangeCooldown(0.4f);
    }

    public override void Update()
    {
        base.Update();
        if (CanExit) StateMachine.ChangeState(StateMachine.GroundState);
    }

    public override void Exit()
    {
        base.Exit();
        Animator.SetBool(IsSwordAttack, false);
        StateMachine.CurrentWeaponDurability--;
    }
}