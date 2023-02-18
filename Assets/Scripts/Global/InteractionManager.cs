using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject dialogueControllerPrefab;
    private Transform canvas;
    public static InteractionManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartDialogue(NPC npc, SO_Dialogue dialogue)
    {
        if (UIManager.instance.interactingWithUI) return;
        UIManager.instance.interactingWithUI = true;
        UIManager.instance.uiState = UIManager.UIStates.Talking;
        
        var newDialogueController = Instantiate(dialogueControllerPrefab, canvas.transform).GetComponent<DialogueController>();

        newDialogueController.dialogue = dialogue;
        newDialogueController.npcInteracting = npc;
    }
    
    public void StartInteraction(Interactive interactiveObject,SO_Dialogue dialogue)
    {
        if (UIManager.instance.interactingWithUI) return;
        UIManager.instance.interactingWithUI = true;
        UIManager.instance.uiState = UIManager.UIStates.Talking;
        
        var newDialogueController = Instantiate(dialogueControllerPrefab, canvas.transform).GetComponent<DialogueController>();

        newDialogueController.dialogue = dialogue;
        newDialogueController.objectInteracting = interactiveObject;
    }
}
