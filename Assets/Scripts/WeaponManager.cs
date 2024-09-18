using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    public StateMachine stateMachine;

    public DamageDealer damageDealer;

    public GameObject[] weapons;

    private int currentWeaponIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ActivateWeaponAtIndex(int index)
    {
        if (currentWeaponIndex != 0)
        {
            DeactivateWeapon();
        }

        // Use the weapon
        weapons[index].SetActive(true);
        currentWeaponIndex = index;
        stateMachine.WearWeapon(index); //durability
        damageDealer.WearWeapon(index); //damage
    }

    public void DeactivateWeapon()
    {
        //0 means fist
        weapons[currentWeaponIndex].SetActive(false);
        currentWeaponIndex = 0;
        stateMachine.WearWeapon(0);
        damageDealer.WearWeapon(0);
    }
}