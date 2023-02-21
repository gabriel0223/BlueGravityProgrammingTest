using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveShop : MonoBehaviour, IInteractive
{
    public static event Action<InteractiveShop> OnShoppingStart;

    [SerializeField] private List<EquipmentData> _itemsForSale;

    public void Interact(Transform playerTransform)
    {
        OnShoppingStart?.Invoke(this);
    }

    public void OnInteractionComplete()
    {
    }

    public List<EquipmentData> GetItems()
    {
        return _itemsForSale;
    }

    public void SetItems(List<EquipmentData> newItemsForSale)
    {
        _itemsForSale = new List<EquipmentData>(newItemsForSale);
    }
}
