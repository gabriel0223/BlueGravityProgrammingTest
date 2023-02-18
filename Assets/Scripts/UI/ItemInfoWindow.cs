using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoWindow : MonoBehaviour
{
    private Rect rectTransform;
    private InventoryController inventoryController;
    [Header("REFERENCES")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemPriceText;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>().rect;
        inventoryController = UIManager.instance.inventoryController;
    }

    private void OnEnable()
    {
        FollowMouse();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }
    
    private void FollowMouse()
    {
        Vector3 offset;
        
        if (!inventoryController.isCarryingItem)
        {
            offset = new Vector3(rectTransform.width, -rectTransform.height * 1.5f) / 1.8f;
        }
        else
        {
            offset = new Vector3(rectTransform.width, -rectTransform.height * 1.5f) / 1.5f;
        }

        //keep window from leaving the screen
        Vector3 newPos = Input.mousePosition + offset;
        newPos.x = Mathf.Clamp(newPos.x, 0 + rectTransform.width / 2, Screen.width - rectTransform.width / 2);
        newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

        transform.position = newPos;
    }

    public void UpdateItemInfo(string itemName, string itemDescription, int itemPrice)
    {
        itemNameText.SetText(itemName);
        itemDescriptionText.SetText(itemDescription);
        itemPriceText.SetText(itemPrice.ToString());
    }
}
