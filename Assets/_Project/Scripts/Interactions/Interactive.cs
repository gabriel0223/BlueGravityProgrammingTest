using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactive : MonoBehaviour, IInteractive
{
    [Tooltip("The dialogue that will be triggered when the player interacts with this object")]
    [SerializeField] private DialogueData dialogueData;
    
    [Tooltip("The dialogue that will be triggered if the player already interacted with object")]
    [SerializeField] private DialogueData interactedDialogueData;
    
    [Tooltip("Does this interaction trigger an event?")]
    [SerializeField] public bool triggerEvent;

    public UnityEvent interactionEvent;

    [HideInInspector] public bool interacted;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(Transform playerTransform)
    {
        //trigger regular dialogue if it's the first interaction, but triggers the interacted dialogue if it's not
        DialogueSystem.instance.StartInteraction(this, interacted? interactedDialogueData : dialogueData);
        interacted = true;
    }

    public void OnInteractionComplete()
    {
        throw new System.NotImplementedException();
    }
}
