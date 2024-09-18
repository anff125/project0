using Card;
using UnityEngine;
using UnityEngine.Serialization;

public class DamageDealer : MonoBehaviour
{
    // animation event
    // for melee weapon

    public string targetTag = "Damageable";

    [FormerlySerializedAs("fistDamageCollider")] [FormerlySerializedAs("Fist Damage Collider")]
    public Collider DamageCollider;

    private int currentWeaponIndex = 0;
    private int currentattackDamage = 0;


    public void WearWeapon(int index)
    {
        currentWeaponIndex = index;
        var weaponCard = CardLaboratory.Instance.cardlist[index] as CardType_Weapon;
        if (weaponCard != null)
        {
            currentattackDamage = weaponCard.AttackDamage;
        }
        else
        {
            Debug.Log("Card is not a weapon card!");
        }
    }

    public void UseSpell(int index)
    {
        currentWeaponIndex = index;
        currentattackDamage = 10;
    }

    private void Start()
    {
        if (DamageCollider == null)
            Debug.LogError("Collider not found on DamageDealer object!");
        else
            DamageCollider.enabled = false;
    }

    public void UpdateDamageCollider()
    {
        if (currentWeaponIndex >= 0 && currentWeaponIndex < CardLaboratory.Instance.cardlist.Length)
        {
            var weaponCollider = WeaponManager.Instance.weapons[currentWeaponIndex];
            if (weaponCollider != null)
            {
                // Assuming each weapon has a specific collider associated with it
                DamageCollider = weaponCollider.GetComponent<Collider>();
            }
        }
    }

    public void CheckForHit()
    {
        UpdateDamageCollider();

        if (DamageCollider == null) return;

        DamageCollider.enabled = true;

        // Declare the results array with an estimated size
        var results = new Collider[10]; // Adjust the size based on your needs

        // Store the number of colliders found in size
        var size = Physics.OverlapBoxNonAlloc(DamageCollider.bounds.center, DamageCollider.bounds.extents,
            results, DamageCollider.transform.rotation, LayerMask.GetMask(targetTag));

        // Iterate over the results based on the size returned
        for (var i = 0; i < size; i++)
        {
            var hitCollider = results[i];
            var targetHealth = hitCollider.GetComponent<Health>();
            if (targetHealth != null) targetHealth.TakeDamage(currentattackDamage);
        }

        DamageCollider.enabled = false;
    }
}