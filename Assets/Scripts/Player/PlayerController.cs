using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform playerSprite;
    private PlayerFootsteps playerFootsteps;
    
    [SerializeField] private float speed;
    [SerializeField] private float interactionRange;
    [Tooltip("The offset in the origin of the interaction raycast")]
    [SerializeField] private Vector2 interactionOriginOffset;
    
    private Vector2 moveInput;
    [HideInInspector] public int directionFacing = 1;
    private bool inputLocked;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerFootsteps = GetComponentInChildren<PlayerFootsteps>();
        playerSprite = GetComponentInChildren<Animator>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputLocked) return;
        
        //get and normalize input
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed;

        SpriteFlip();
        Interaction();
    }

    private void FixedUpdate()
    {
        if (inputLocked) return;
        
        Movement();
    }
    
    private void SpriteFlip()
    {
        if (moveInput.x == 0) return;

        //flip player sprite according to input
        playerSprite.localScale = new Vector2(moveInput.x > 0 ? 1 : -1, playerSprite.localScale.y);
        directionFacing = (int)playerSprite.localScale.x;
    }

    private void Movement()
    {
        rb.velocity = moveInput;
    }

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
