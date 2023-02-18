using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> OnMove;

    private MyInputActions _inputActions;

    private void Awake()
    {
        _inputActions = new MyInputActions();

        _inputActions.Gameplay.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
}
