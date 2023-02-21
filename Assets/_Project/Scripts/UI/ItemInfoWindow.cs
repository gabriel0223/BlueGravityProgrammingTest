using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoWindow : MonoBehaviour
{
    private Rect rectTransform;
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;
    [SerializeField] private TextMeshProUGUI _itemPriceText;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>().rect;
    }

    void Update()
    {
        FollowMouse();
    }
    
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

    private void FollowMouse()
    {
        // Vector3 offset;
        //
        // if (!_inventoryView.IsDraggingItem)
        // {
        //     offset = new Vector3(rectTransform.width, -rectTransform.height * 1.5f) / 1.8f;
        // }
        // else
        // {
        //     offset = new Vector3(rectTransform.width, -rectTransform.height * 1.5f) / 1.5f;
        // }
        //
        // //keeps window from leaving the screen
        // Vector3 newPos = Input.mousePosition + offset;
        // newPos.x = Mathf.Clamp(newPos.x, 0 + rectTransform.width / 2, Screen.width - rectTransform.width / 2);
        // newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);
        //
        // transform.position = newPos;
    }
}
