using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public Vector2 NavigationInput { get; private set; }

    private InputAction _navaigateAction;

    public static PlayerInput PlayerInput { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        PlayerInput = GetComponent<PlayerInput>();
        _navaigateAction = PlayerInput.actions["Navigate"];
    }

    private void Update()
    {
        NavigationInput = _navaigateAction.ReadValue<Vector2>();
    }
}