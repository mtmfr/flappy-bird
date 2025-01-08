using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions inputs;

    private void Awake()
    {
        inputs = new();
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Jump.performed += JumpTrigger;
    }

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Player.Jump.performed -= JumpTrigger;
    }

    public static event Action JumpEvent;

    private void JumpTrigger(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke();
    }
}
