using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractive
{
    public void Interact(Transform playerTransform);

    public void OnInteractionComplete();
}
