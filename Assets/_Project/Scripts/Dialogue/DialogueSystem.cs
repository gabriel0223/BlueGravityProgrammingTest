using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public event Action OnStartDialogue;
    public event Action OnEndDialogue;

    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Transform _canvas;
    [SerializeField] private DialogueView _dialogueViewPrefab;

    public static DialogueSystem instance;
    private IInteractive _currentInteraction;

    // Start is called before the first frame update
    void Start()
    {
        NpcController.OnDialogueStart += StartNpcDialogue;
    }

    private void OnDestroy()
    {
        NpcController.OnDialogueStart -= StartNpcDialogue;
    }

    private void StartNpcDialogue(NpcController npc, DialogueData dialogueData)
    {
        if (UIManager.instance.interactingWithUI) return;
        UIManager.instance.interactingWithUI = true;
        UIManager.instance.uiState = UIManager.UIStates.Talking;

        _currentInteraction = npc;

        DialogueView newDialogueView = Instantiate(_dialogueViewPrefab, _canvas.transform);

        newDialogueView.SetDialogueData(dialogueData);
        newDialogueView.SetOnComplete(EndNpcDialogue);
        newDialogueView.SetInputManager(_inputManager);

        OnStartDialogue?.Invoke();
    }

    private void EndNpcDialogue()
    {
        _currentInteraction.OnInteractionComplete();
        _currentInteraction = null;

        OnEndDialogue?.Invoke();
    }

    public void StartDialogue(NpcController npc, DialogueData dialogueData)
    {
        if (UIManager.instance.interactingWithUI) return;
        UIManager.instance.interactingWithUI = true;
        UIManager.instance.uiState = UIManager.UIStates.Talking;

        var newDialogueController = Instantiate(_dialogueViewPrefab, _canvas.transform);

        newDialogueController.SetDialogueData(dialogueData);
    }
    
    public void StartInteraction(Interactive interactiveObject,DialogueData dialogueData)
    {
        if (UIManager.instance.interactingWithUI) return;
        UIManager.instance.interactingWithUI = true;
        UIManager.instance.uiState = UIManager.UIStates.Talking;

        var newDialogueController = Instantiate(_dialogueViewPrefab, _canvas.transform);

        newDialogueController.SetDialogueData(dialogueData);
    }
}
