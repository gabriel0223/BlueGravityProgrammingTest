using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NpcController : MonoBehaviour, IInteractive
{
    public static event Action<NpcController, DialogueData> OnDialogueStart;

    [SerializeField] private GameObject _newInteractionIcon;
    [SerializeField] private Transform _npcSprites;

    [Header("List of dialogues with this NPC")] 
    [SerializeField] private DialogueData[] _dialogueList;

    private Queue<DialogueData> _dialogueQueue;
    private bool _isInteracting;

    private void Awake()
    {
        _dialogueQueue = new Queue<DialogueData>(_dialogueList);

        UpdateInteractionSign();
    }

    public void Interact(Transform playerTransform)
    {
        if (_dialogueQueue.Count == 0 || _isInteracting)
        {
            return;
        }

        _isInteracting = true;

        FlipSpriteToFacePlayer(playerTransform);
        SetInteractionSign(false);

        OnDialogueStart?.Invoke(this, _dialogueQueue.Dequeue());
    }

    private void FlipSpriteToFacePlayer(Transform player)
    {
        Vector3 playerDirection = player.position - transform.position;
        _npcSprites.localScale = new Vector2(playerDirection.x > 0 ? 1 : -1, _npcSprites.localScale.y);
    }

    private void SetInteractionSign(bool value)
    {
        _newInteractionIcon.SetActive(value);
    }

    private void UpdateInteractionSign()
    {
        SetInteractionSign(_dialogueQueue.Count > 0);
    }

    public void OnInteractionComplete()
    {
        _isInteracting = false;

        UpdateInteractionSign();
    }
}
