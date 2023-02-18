using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Purchasable : MonoBehaviour
{
    [SerializeField] private Image purchasableIcon;
    [SerializeField] private TextMeshProUGUI purchasableName;
    [SerializeField] private TextMeshProUGUI purchasablePrice;
    private InventoryController inventoryController;
    private ShopController shopController;
    private PlayerMoney playerMoney;
    
    [Space(10)]
    public SO_Equipment item;

    private void Awake()
    {
        inventoryController = UIManager.instance.inventoryController;
        shopController = UIManager.instance.shopController;
    }

    private void OnEnable()
    {
        UpdatePurchasableUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMoney = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoney>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdatePurchasableUI()
    {
        purchasableIcon.sprite = item.equipmentSprite;
        purchasableName.SetText(item.name);
        purchasablePrice.SetText(item.purchasePrice.ToString());

        float iconScale;
        Vector2 iconPosition;
        
        //ADJUST ICON SCALE AND POSITION
        switch (item.equipmentType)
        {
            case SO_Equipment.EquipmentType.Top:
                iconScale = 3.3f;
                iconPosition = new Vector2(-272, 62);
                break;
            case SO_Equipment.EquipmentType.Head:
                iconScale = 2.6f;
                iconPosition = new Vector2(-272, -20);
                break;
            case SO_Equipment.EquipmentType.Bottom:
                iconScale = 3.3f;
                iconPosition = new Vector2(-272, 103);
                break;
            case SO_Equipment.EquipmentType.Face:
                iconScale = 3.3f;
                iconPosition = new Vector2(-275, 4);
                break;
            default: //weapon
                iconScale = 3.7f;
                iconPosition = new Vector2(-316, 132);
                break;
        }
        
        purchasableIcon.transform.localScale = Vector3.one * iconScale;
        purchasableIcon.GetComponent<RectTransform>().localPosition = iconPosition;
    }

    public void Purchase()
    {
        if (playerMoney.playerCoins < item.purchasePrice)
        {
            transform.DOShakePosition(0.25f, 5);
            AudioManager.instance.Play("Error");
        }
        else
        {
            AudioManager.instance.Play("ItemPurchase");
            inventoryController.GetFirstEmptySlot().AddItem(item, false);
            playerMoney.UpdateCoins(-item.purchasePrice);
            shopController.RemoveItemFromShop(item);
            transform.DOScaleY(0, 0.5f).OnComplete(() => Destroy(gameObject));
        }
    }
}
