using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;
    [SerializeField] private TextMeshProUGUI _itemPriceText;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void UpdateItemInfo(string itemName, string itemDescription, int itemPrice)
    {
        _itemNameText.SetText(itemName);
        _itemDescriptionText.SetText(itemDescription);
        _itemPriceText.SetText(itemPrice.ToString());
    }
}
