using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;

    private Rigidbody2D rb;
    private Transform playerSprite;
    private PlayerFootsteps playerFootsteps;
    
    [SerializeField] private float speed;
    [SerializeField] private float interactionRange;
    [Tooltip("The offset in the origin of the interaction raycast")]
    [SerializeField] private Vector2 interactionOriginOffset;
    
    [HideInInspector] public int directionFacing = 1;
    private bool inputLocked;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerFootsteps = GetComponentInChildren<PlayerFootsteps>();
        playerSprite = GetComponentInChildren<Animator>().transform;
    }

    private void Start()
    {
        _inputManager.OnMove += HandleMovement;
    }

    private void HandleMovement(Vector2 moveInput)
    {
        if (inputLocked)
        {
            return;
        }

        Vector2 newSpeed = moveInput.normalized * speed;

        rb.velocity = newSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputLocked) return;

        //SpriteFlip();
        Interaction();
    }

    // private void SpriteFlip()
    // {
    //     if (moveInput.x == 0) return;
    //
    //     //flip player sprite according to input
    //     playerSprite.localScale = new Vector2(moveInput.x > 0 ? 1 : -1, playerSprite.localScale.y);
    //     directionFacing = (int)playerSprite.localScale.x;
    // }

    private void Interaction()
    {
        if (Input.GetButtonDown("Interact"))
        {
            var hitInfo = Physics2D.Raycast(transform.position + (Vector3)interactionOriginOffset,
                Vector3.right * directionFacing, interactionRange);

            var interactable = hitInfo.collider.GetComponent<IInteractive>();
            
            if (interactable == null) return;
            
            interactable.Interact();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (Vector3)interactionOriginOffset, Vector3.right * directionFacing * interactionRange);
    }

    public void LockInput(bool locked)
    {
        if (locked) rb.velocity = Vector2.zero;
        inputLocked = locked;
    }
}
