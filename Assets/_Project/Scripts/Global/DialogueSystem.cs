using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private Transform _canvas;
    [SerializeField] private DialogueController _dialogueControllerPrefab;

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

        DialogueController newDialogueController = Instantiate(_dialogueControllerPrefab, _canvas.transform);

        newDialogueController.dialogueData = dialogueData;
        newDialogueController.OnComplete = EndNpcDialogue;
    }

    private void EndNpcDialogue()
    {
        _currentInteraction.OnInteractionComplete();

        _currentInteraction = null;
    }

    public void StartDialogue(NpcController npc, DialogueData dialogueData)
    {
        if (UIManager.instance.interactingWithUI) return;
        UIManager.instance.interactingWithUI = true;
        UIManager.instance.uiState = UIManager.UIStates.Talking;

        var newDialogueController = Instantiate(_dialogueControllerPrefab, _canvas.transform);

        newDialogueController.dialogueData = dialogueData;
    }
    
    public void StartInteraction(Interactive interactiveObject,DialogueData dialogueData)
    {
        if (UIManager.instance.interactingWithUI) return;
        UIManager.instance.interactingWithUI = true;
        UIManager.instance.uiState = UIManager.UIStates.Talking;

        var newDialogueController = Instantiate(_dialogueControllerPrefab, _canvas.transform);

        newDialogueController.dialogueData = dialogueData;
        newDialogueController.objectInteracting = interactiveObject;
    }
}
