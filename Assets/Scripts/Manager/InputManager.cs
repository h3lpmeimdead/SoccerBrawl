using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 _movement;
    public static bool _jumpWasPressed;
    public static bool _jumpIsHeld;
    public static bool _jumpWasReleased;
    public static bool _kickWasPressed;
    public static bool _kickWasReleased;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _kickAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        _moveAction = PlayerInput.actions["Move"];
        _jumpAction = PlayerInput.actions["Jump"];
        _kickAction = PlayerInput.actions["Kick"];
    }

    private void Update()
    {
        _movement = _moveAction.ReadValue<Vector2>();
        _jumpWasPressed = _jumpAction.WasPressedThisFrame();
        _jumpIsHeld = _jumpAction.IsPressed();
        _jumpWasReleased = _jumpAction.WasReleasedThisFrame();
        _kickWasPressed = _kickAction.WasPressedThisFrame();
        _kickWasReleased = _kickAction.WasReleasedThisFrame();
    }
}
