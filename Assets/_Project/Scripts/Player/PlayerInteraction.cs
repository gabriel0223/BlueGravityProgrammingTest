using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private LayerMask _interactiveLayerMask;
    [SerializeField] private float _interactionDistanceRange;
    [SerializeField] private float _interactionRadius;

    private int _directionFacing = 1;

    void Start()
    {
        _inputManager.OnInteract += HandleInteraction;
        _inputManager.OnMove += HandleDirectionFacing;
    }

    private void HandleDirectionFacing(Vector2 newInput)
    {
        if (newInput.x == 0)
        {
            return;
        }

        _directionFacing = newInput.x > 0 ? 1 : -1;
    }

    private void HandleInteraction()
    {
        Vector3 interactionDistance = Vector3.right * (_directionFacing * _interactionDistanceRange);

        Collider2D hitInfo = Physics2D.OverlapCircle(transform.position + interactionDistance,
            _interactionRadius, _interactiveLayerMask);

        if (hitInfo == null)
        {
            return;
        }

        IInteractive interactive = hitInfo.gameObject.GetComponent<IInteractive>();

        interactive.Interact(transform);
    }

    private void OnDrawGizmos()
    {
        Vector3 interactionDistance = Vector3.right * (_directionFacing * _interactionDistanceRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + interactionDistance,  _interactionRadius);
    }
}
