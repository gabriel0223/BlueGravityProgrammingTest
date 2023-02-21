using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveShop : MonoBehaviour, IInteractive
{
    public static event Action<InteractiveShop> OnShoppingStart;

    [SerializeField] private List<SO_Equipment> _itemsForSale;

    public void Interact(Transform playerTransform)
    {
        OnShoppingStart?.Invoke(this);
    }

    public void OnInteractionComplete()
    {
    }

    public List<SO_Equipment> GetItems()
    {
        return _itemsForSale;
    }

    public void SetItems(List<SO_Equipment> newItemsForSale)
    {
        _itemsForSale = new List<SO_Equipment>(newItemsForSale);
    }
}
