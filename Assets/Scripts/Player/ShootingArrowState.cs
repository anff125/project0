using System.Collections;
using UnityEngine;

public class ShootingArrowState : StateTemplate
{
    private static readonly int IsShoot = Animator.StringToHash("isShoot");

    public ShootingArrowState(GameObject player, Animator animator, CharacterController controller,
        StateMachine stateMachine)
        : base(player, animator, controller, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (StateMachine.CurrentWeaponIndex != 2)
        {
            Debug.Log("Weapon is not bow");
            // stop game
            throw new System.NotImplementedException();
        }

        Animator.SetBool(IsShoot, true);
        SetStateChangeCooldown(0.2f);
        ShootArrow();
    }

    public override void Update()
    {
        base.Update();

        StateMachine.ChangeState(StateMachine.GroundState);
    }

    public override void Exit()
    {
        base.Exit();
        Animator.SetBool(IsShoot, false);
        StateMachine.CurrentWeaponDurability--;
    }

    private void ShootArrow()
    {
        //Debug.Log("Arrow shot!");
        GameObject arrow = StateMachine.arrowPool.GetObject();
        arrow.transform.position = StateMachine.leftHand.position;

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 aimDirection = Camera.main.transform.forward;
        float maxAimDistance = 10f; // Adjust as needed
        var aimPoint = cameraPosition + aimDirection * maxAimDistance;

        Vector3 shootDirection = (aimPoint - arrow.transform.position).normalized;

        arrow.transform.rotation = Quaternion.LookRotation(shootDirection) * Quaternion.Euler(90, 0, 0);
        arrow.GetComponent<Rigidbody>().velocity = shootDirection * 100f; // Adjust speed as needed

        // Add ArrowCollisionHandler if not already present
        if (arrow.GetComponent<ArrowCollisionHandler>() == null)
        {
            arrow.AddComponent<ArrowCollisionHandler>();
        }

        // Start coroutine to return the arrow after 1 second
        StateMachine.StartCoroutine(ReturnArrowAfterDelay(arrow, 1f));
    }

    private IEnumerator ReturnArrowAfterDelay(GameObject arrow, float delay)
    {
        yield return new WaitForSeconds(delay);
        StateMachine.arrowPool.ReturnObject(arrow);
    }
}