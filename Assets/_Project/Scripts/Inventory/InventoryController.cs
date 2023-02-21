using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using EquipmentType = EquipmentData.EquipmentType;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private PlayerMoneyController _playerMoney;
    [SerializeField] private InventoryView _inventoryView;
    [Space]
    [SerializeField] private List<EquipmentData> _initialInventory = new();
    [SerializedDictionary("EquipmentType", "Equipment")]
    [SerializeField] private SerializedDictionary<EquipmentType, EquipmentData> _initialEquippedItems = new();

    private List<EquipmentData> _inventory = new();
    private Dictionary<EquipmentType, EquipmentData> _equippedItems = new();

    private void Start()
    {
        Initialize();

        _inventoryView.OnPlayerEquipItem += HandleItemEquipped;
        _inventoryView.OnPlayerSellItem += HandleItemSold;

        ShopView.OnPlayerBuyItem += HandleItemPurchased;
    }

    private void OnDestroy()
    {
        _inventoryView.OnPlayerEquipItem -= HandleItemEquipped;
        _inventoryView.OnPlayerSellItem -= HandleItemSold;

        ShopView.OnPlayerBuyItem -= HandleItemPurchased;
    }

    private void Initialize()
    {
        _inventory = new List<EquipmentData>(_initialInventory);
        _equippedItems = new Dictionary<EquipmentType, EquipmentData>(_initialEquippedItems);

        InitializeInventoryUi();
    }

    private void HandleItemEquipped(EquipmentData newEquippedItem)
    {
        _inventory.Remove(newEquippedItem);

        _equippedItems.TryGetValue(newEquippedItem.equipmentType, out EquipmentData itemToBeReplaced);

        if (itemToBeReplaced != null)
        {
            _inventory.Add(itemToBeReplaced);
        }

        _equippedItems[newEquippedItem.equipmentType] = newEquippedItem;
    }

    private void InitializeInventoryUi()
    {
        _inventoryView.Initialize();

        foreach (EquipmentData item in _initialInventory)
        {
            _inventoryView.AddItem(item);
        }

        foreach (EquipmentData equipment in _initialEquippedItems.Values)
        {
            _inventoryView.AddEquipment(equipment);
        }
    }

    private void HandleItemSold(EquipmentData soldItem)
    {
        _inventory.Remove(soldItem);
        _playerMoney.AddMoney(soldItem.sellingPrice);

        AudioManager.instance.Play(Sounds.ItemSell);
    }

    private void HandleItemPurchased(EquipmentData newItem)
    {
        _inventory.Add(newItem);
        _playerMoney.SubtractMoney(newItem.purchasePrice);

        AudioManager.instance.Play(Sounds.ItemPurchase);
    }
}
