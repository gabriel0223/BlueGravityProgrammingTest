using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObjectController : MonoBehaviour, IInteractive
{
    public static event Action<InteractiveObjectController, DialogueData> OnDialogueStart;

    [Tooltip("The dialogue that will be triggered when the player interacts with this object")]
    [SerializeField] private DialogueData _dialogueData;
    [Tooltip("The dialogue that will be triggered if the player interacts with the object again")]
    [SerializeField] private DialogueData _repeatedInteractionDialogueData;
    [SerializeField] private UnityEvent _onInteractionComplete;

    private bool _hasBeenInteracted;

    public void Interact(Transform playerTransform)
    {
        OnDialogueStart?.Invoke(this, _hasBeenInteracted? _repeatedInteractionDialogueData : _dialogueData);
    }

    public void OnInteractionComplete()
    {
        if (_hasBeenInteracted)
        {
            return;
        }
        
        _hasBeenInteracted = true;

        _onInteractionComplete.Invoke();
    }
}
