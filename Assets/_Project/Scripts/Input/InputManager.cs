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
    public event Action OnEscape;
    public event Action OnReturn;

    [SerializeField] private DialogueSystem _dialogueSystem;
    [SerializeField] private UIManager _uiManager;

    private MyInputActions _inputActions;

    public Vector2 MoveInput { get; private set; }

    private void Awake()
    {
        _inputActions = new MyInputActions();

        _inputActions.Gameplay.Move.performed += HandleMoveInput;
        _inputActions.Gameplay.Interact.performed += HandleInteractInput;
        _inputActions.Gameplay.Escape.performed += HandleEscapeInput;

        _inputActions.UI.Confirm.performed += HandleConfirmInput;
        _inputActions.UI.Return.performed += HandleReturnInput;

        _dialogueSystem.OnStartDialogue += SwitchToUIActionMap;
        _dialogueSystem.OnEndDialogue += SwitchToGameplayActionMap;

        _uiManager.OnOpenMenu += SwitchToUIActionMap;
        _uiManager.OnCloseMenu += SwitchToGameplayActionMap;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        MoveInput = _inputActions.Gameplay.Move.ReadValue<Vector2>();
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
        _inputActions.Gameplay.Escape.performed -= HandleEscapeInput;

        _inputActions.UI.Confirm.performed -= HandleConfirmInput;

        _dialogueSystem.OnStartDialogue -= SwitchToUIActionMap;
        _dialogueSystem.OnEndDialogue -= SwitchToGameplayActionMap;

        _uiManager.OnOpenMenu -= SwitchToUIActionMap;
        _uiManager.OnCloseMenu -= SwitchToGameplayActionMap;
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

    private void HandleEscapeInput(InputAction.CallbackContext obj)
    {
        OnEscape?.Invoke();
    }

    private void HandleReturnInput(InputAction.CallbackContext obj)
    {
        OnReturn?.Invoke();
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
