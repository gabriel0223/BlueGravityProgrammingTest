using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action OnInteract; 
    public event Action OnConfirm;

    [SerializeField] private DialogueSystem _dialogueSystem;

    private MyInputActions _inputActions;

    private void Awake()
    {
        _inputActions = new MyInputActions();

        _inputActions.Gameplay.Move.performed += HandleMoveInput;
        _inputActions.Gameplay.Interact.performed += HandleInteractInput;
        _inputActions.UI.Confirm.performed += HandleConfirmInput;

        _dialogueSystem.OnStartDialogue += SwitchToUIActionMap;
        _dialogueSystem.OnEndDialogue += SwitchToGameplayActionMap;
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void OnDestroy()
    {
        _inputActions.Gameplay.Move.performed -= HandleMoveInput;
        _inputActions.Gameplay.Interact.performed -= HandleInteractInput;
        _inputActions.UI.Confirm.performed -= HandleConfirmInput;

        _dialogueSystem.OnStartDialogue -= SwitchToUIActionMap;
        _dialogueSystem.OnEndDialogue -= SwitchToGameplayActionMap;
    }

    private void HandleMoveInput(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void HandleInteractInput(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke();
    }

    private void HandleConfirmInput(InputAction.CallbackContext ctx)
    {
        OnConfirm?.Invoke();
    }

    private void Initialize()
    {
        _inputActions.Gameplay.Enable();
        _inputActions.UI.Disable();
    }

    private void SwitchToUIActionMap()
    {
        _inputActions.Gameplay.Disable();
        _inputActions.UI.Enable();
    }

    private void SwitchToGameplayActionMap()
    {
        _inputActions.UI.Disable();
        _inputActions.Gameplay.Enable();
    }
}
