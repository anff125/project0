using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public UnityEvent onDeath;
    public UnityEvent<int, int> onHealthChanged;
    public float blinkDuration = 1f;
    public float blinkInterval = 0.1f;
    [FormerlySerializedAs("My Renderer")] public Renderer[] myRenderers;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        onHealthChanged.Invoke(currentHealth, maxHealth);

        if (currentHealth == 0)
            Die();
        else
            StartCoroutine(BlinkEffect());
    }

    private IEnumerator BlinkEffect()
    {
        var endTime = Time.time + blinkDuration;
        while (Time.time < endTime)
        {
            foreach (var renderer1 in myRenderers) renderer1.enabled = !renderer1.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        foreach (var renderer1 in myRenderers) renderer1.enabled = true; // Ensure all renderers are enabled when done
    }

    private void Die()
    {
        onDeath.Invoke();
        gameObject.SetActive(false);
    }
}