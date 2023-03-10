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

    private IInteractive _currentInteraction;

    void Start()
    {
        NpcController.OnDialogueStart += StartNpcDialogue;
        InteractiveObjectController.OnDialogueStart += StartInteractionDialogue;
    }

    private void OnDestroy()
    {
        NpcController.OnDialogueStart -= StartNpcDialogue;
        InteractiveObjectController.OnDialogueStart -= StartInteractionDialogue;
    }

    private void StartNpcDialogue(NpcController npc, DialogueData dialogueData)
    {
        _currentInteraction = npc;

        DialogueView newDialogueView = Instantiate(_dialogueViewPrefab, _canvas.transform);

        newDialogueView.SetDialogueData(dialogueData);
        newDialogueView.SetOnComplete(EndDialogue);
        newDialogueView.SetInputManager(_inputManager);

        OnStartDialogue?.Invoke();
    }

    private void StartInteractionDialogue(InteractiveObjectController obj, DialogueData dialogueData)
    {
        _currentInteraction = obj;

        DialogueView newDialogueView = Instantiate(_dialogueViewPrefab, _canvas.transform);

        newDialogueView.SetDialogueData(dialogueData);
        newDialogueView.SetOnComplete(EndDialogue);
        newDialogueView.SetInputManager(_inputManager);

        OnStartDialogue?.Invoke();
    }

    private void EndDialogue()
    {
        _currentInteraction.OnInteractionComplete();
        _currentInteraction = null;

        OnEndDialogue?.Invoke();
    }
}
