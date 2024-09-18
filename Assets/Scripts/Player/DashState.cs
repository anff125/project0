using UnityEngine;

namespace Player
{
    public class DashState : StateTemplate
    {
        private static readonly int IsDash = Animator.StringToHash("isDash");

        public DashState(GameObject player, Animator animator, CharacterController controller,
            StateMachine stateMachine) : base(player, animator, controller, stateMachine)
        {
        }
        
        private Vector3 _dashDirection;
        public override void Enter()
        {
            base.Enter();
            Animator.SetBool(IsDash, true);
            SetStateChangeCooldown(0.1f);

            // Set the dash direction and speed
            Vector3 dashDirection = Player.transform.forward;
            float dashSpeed = 20f; // Adjust the dash speed as needed

            // Apply the dash movement
            _dashDirection = dashDirection * dashSpeed;
        }

        public override void Update()
        {
            base.Update();
            StateMachine.ChangeState(StateMachine._previousState);
            // Move the player using CharacterController
            Controller.Move(_dashDirection * Time.deltaTime);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
        }

        public override void Exit()
        {
            base.Exit();
            Animator.SetBool(IsDash, false);
        }
    }
}