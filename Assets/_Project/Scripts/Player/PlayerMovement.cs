using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _movementSpeed;

    private bool inputLocked;

    private void Start()
    {
        _inputManager.OnMove += HandleMovement;
    }

    private void HandleMovement(Vector2 moveInput)
    {
        if (inputLocked)
        {
            return;
        }

        Vector2 newSpeed = moveInput.normalized * _movementSpeed;
        _rb.velocity = newSpeed;
    }

    public void LockInput(bool locked)
    {
        if (locked) _rb.velocity = Vector2.zero;
        inputLocked = locked;
    }
}
