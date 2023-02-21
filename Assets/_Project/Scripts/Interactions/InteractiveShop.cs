using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveShop : MonoBehaviour, IInteractive
{
    public static event Action<InteractiveShop> OnShoppingStart;

    [Header("list of items being sold in this shop")]
    public List<SO_Equipment> _items;

    public void Interact(Transform playerTransform)
    {
        OnShoppingStart?.Invoke(this);
    }

    public void OnInteractionComplete()
    {
        throw new NotImplementedException();
    }

    public List<SO_Equipment> GetItems()
    {
        return _items;
    }
}
