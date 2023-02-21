using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveShop : MonoBehaviour, IInteractive
{
    [SerializeField] private ShopController _shopController;
    
    [Header("list of items being sold in this shop")]
    public List<SO_Equipment> _items;

    public void Interact(Transform playerTransform)
    {
        _shopController.OpenShop(this, _items);
    }

    public void OnInteractionComplete()
    {
        throw new NotImplementedException();
    }
}
