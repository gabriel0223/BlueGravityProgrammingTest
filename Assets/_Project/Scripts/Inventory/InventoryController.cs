using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using EquipmentType = EquipmentData.EquipmentType;

public class InventoryController : MonoBehaviour
{
    public static event Action<EquipmentData> OnPlayerSellItem;

    [SerializeField] private PlayerMoneyController _playerMoney;
    [Space]
    [SerializeField] private List<EquipmentData> _initialInventory = new();
    [SerializedDictionary("EquipmentType", "Equipment")]
    [SerializeField] private SerializedDictionary<EquipmentType, EquipmentData> _initialEquippedItems = new();

    public List<EquipmentData> Inventory { get; private set; }
    public Dictionary<EquipmentType, EquipmentData> EquippedItems { get; private set; }

    private void Start()
    {
        Initialize();

        ShopView.OnPlayerBuyItem += HandleItemPurchased;
    }

    private void OnDestroy()
    {
        ShopView.OnPlayerBuyItem -= HandleItemPurchased;
    }

    private void Initialize()
    {
        Inventory = new List<EquipmentData>(_initialInventory);
        EquippedItems = new Dictionary<EquipmentType, EquipmentData>(_initialEquippedItems);
    }

    public void EquipItem(EquipmentData newEquippedItem)
    {
        Inventory.Remove(newEquippedItem);

        EquippedItems.TryGetValue(newEquippedItem.equipmentType, out EquipmentData itemToBeReplaced);

        if (itemToBeReplaced != null)
        {
            Inventory.Add(itemToBeReplaced);
        }

        EquippedItems[newEquippedItem.equipmentType] = newEquippedItem;
    }

    public void SellItem(EquipmentData soldItem)
    {
        Inventory.Remove(soldItem);
        _playerMoney.AddMoney(soldItem.sellingPrice);

        OnPlayerSellItem?.Invoke(soldItem);
        AudioManager.instance.Play(Sounds.ItemSell);
    }

    private void HandleItemPurchased(EquipmentData newItem)
    {
        Inventory.Add(newItem);
        _playerMoney.SubtractMoney(newItem.purchasePrice);

        AudioManager.instance.Play(Sounds.ItemPurchase);
    }
}
