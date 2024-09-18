using UnityEngine;

public class ArrowCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Deal damage to the target
        var targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(10);
        }

        // Return the arrow to the pool when it hits something
        StateMachine.arrowPool.ReturnObject(gameObject);
    }
}