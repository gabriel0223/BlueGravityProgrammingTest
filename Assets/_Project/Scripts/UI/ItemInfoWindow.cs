using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoWindow : MonoBehaviour
{
    private Rect rectTransform;
    
    [Header("REFERENCES")]
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemPriceText;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>().rect;
    }

    // Update is called once per frame
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
        itemNameText.SetText(itemName);
        itemDescriptionText.SetText(itemDescription);
        itemPriceText.SetText(itemPrice.ToString());
    }

    private void FollowMouse()
    {
        Vector3 offset;
        
        if (!_inventoryView.IsCarryingItem)
        {
            offset = new Vector3(rectTransform.width, -rectTransform.height * 1.5f) / 1.8f;
        }
        else
        {
            offset = new Vector3(rectTransform.width, -rectTransform.height * 1.5f) / 1.5f;
        }

        //keeps window from leaving the screen
        Vector3 newPos = Input.mousePosition + offset;
        newPos.x = Mathf.Clamp(newPos.x, 0 + rectTransform.width / 2, Screen.width - rectTransform.width / 2);
        newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

        transform.position = newPos;
    }
}
