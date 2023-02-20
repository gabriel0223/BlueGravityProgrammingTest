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

    private void Start()
    {
        _inputManager.OnMove += HandleMovement;
    }

    private void OnDestroy()
    {
        _inputManager.OnMove -= HandleMovement;
    }

    private void HandleMovement(Vector2 moveInput)
    {
        Vector2 newSpeed = moveInput.normalized * _movementSpeed;
        _rb.velocity = newSpeed;
    }
}
