using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;
    public StateMachine stateMachine;
    public DamageDealer damageDealer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UseSpell(int spellId)
    {
        switch (spellId)
        {
            case 0:
                Debug.Log("Spell: Dash");
                stateMachine.ChangeState(stateMachine.DashState);
                break;
            case 1:
                Debug.Log("Spell: Cyclone");
                stateMachine.ChangeState(stateMachine.CycloneState);
                break;
            default:
                Debug.Log("Spell not found");
                break;
        }
    }
}