using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractive
{
    [SerializeField] private GameObject newInteractionSign;
    private Transform npcSprite;
    private PlayerController player;
    
    [Header("List of dialogues with this NPC")] 
    [SerializeField] private SO_Dialogue[] dialogueList;
    private Queue<SO_Dialogue> dialogueQueue;

    private void Awake()
    {
        dialogueQueue = new Queue<SO_Dialogue>(dialogueList);
        npcSprite = GetComponentInChildren<Animator>().transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        UpdateInteractionSign();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (dialogueQueue.Count == 0) return;
        
        //if the NPC is looking the other way, flip it 
        var npcScale = npcSprite.localScale;
        
        if ((int)npcSprite.localScale.x == player.directionFacing)
            npcSprite.localScale = new Vector3(-npcScale.x, npcScale.y, npcScale.z);
        
        InteractionManager.instance.StartDialogue(this, dialogueQueue.Dequeue());
        UpdateInteractionSign();
    }

    public void SetInteractionSign(bool enabled)
    {
        newInteractionSign.SetActive(enabled);
    }
    public void UpdateInteractionSign()
    {
        //while there are possible conversations left, show interaction sign
        newInteractionSign.SetActive(dialogueQueue.Count > 0);
    }
}
