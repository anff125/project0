using UnityEngine;

public abstract class StateTemplate
{
    protected readonly GameObject Player;
    protected readonly Animator Animator;
    protected readonly CharacterController Controller;
    protected readonly StateMachine StateMachine;
    private float _stateChangeCooldownTimer = 0f; // Cooldown timer
    public bool CanExit => _stateChangeCooldownTimer <= 0;

    protected StateTemplate(GameObject player, Animator animator, CharacterController controller,
        StateMachine stateMachine)
    {
        Player = player;
        Animator = animator;
        Controller = controller;
        StateMachine = stateMachine;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public virtual void Enter()
    {
        //Debug.Log("Entering State: " + GetType().Name);
    }

    public virtual void Update()
    {
        if (_stateChangeCooldownTimer > 0) _stateChangeCooldownTimer -= Time.deltaTime;
    }

    public virtual void FixedUpdate()
    {
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public virtual void Exit()
    {
        //Debug.Log("Exiting State: " + GetType().Name);
    }

    protected void SetStateChangeCooldown(float duration)
    {
        _stateChangeCooldownTimer = duration;
    }
}