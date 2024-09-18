using UnityEngine;

namespace Player
{
    public class CycloneState : StateTemplate
    {
        private static readonly int IsCyclone = Animator.StringToHash("isCyclone");

        public CycloneState(GameObject player, Animator animator, CharacterController controller,
            StateMachine stateMachine) : base(player, animator, controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Animator.SetBool(IsCyclone, true);
            Player.GetComponent<DamageDealer>().UseSpell(4);
            SetStateChangeCooldown(0.1f);
        }

        public override void Update()
        {
            base.Update();
            StateMachine.ChangeState(StateMachine._previousState);

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            Animator.SetBool(IsCyclone, false);
        }
    }
}