using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using EquipmentType = SO_Equipment.EquipmentType;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private PlayerMoneyController _playerMoney;
    [SerializeField] private InventoryView _inventoryView;
    [Space]
    [SerializeField] private List<SO_Equipment> _initialInventory = new();
    [SerializedDictionary("EquipmentType", "Equipment")]
    [SerializeField] private SerializedDictionary<EquipmentType, SO_Equipment> _initialEquippedItems = new();

    private List<SO_Equipment> _inventory = new();
    private Dictionary<EquipmentType, SO_Equipment> _equippedItems = new();

    private void Start()
    {
        Initialize();

        _inventoryView.OnPlayerEquipItem += HandleItemEquipped;
        _inventoryView.OnPlayerSellItem += HandleItemSold;

        ShopController.OnPlayerBuyItem += HandleItemPurchased;
    }

    private void OnDestroy()
    {
        _inventoryView.OnPlayerEquipItem -= HandleItemEquipped;
        _inventoryView.OnPlayerSellItem -= HandleItemSold;

        ShopController.OnPlayerBuyItem -= HandleItemPurchased;
    }

    private void Initialize()
    {
        _inventory = new List<SO_Equipment>(_initialInventory);
        _equippedItems = new Dictionary<EquipmentType, SO_Equipment>(_initialEquippedItems);

        InitializeInventoryUi();
    }

    private void HandleItemEquipped(SO_Equipment newEquippedItem)
    {
        _inventory.Remove(newEquippedItem);

        _equippedItems.TryGetValue(newEquippedItem.equipmentType, out SO_Equipment itemToBeReplaced);

        if (itemToBeReplaced != null)
        {
            _inventory.Add(itemToBeReplaced);
        }

        _equippedItems[newEquippedItem.equipmentType] = newEquippedItem;
    }

    private void InitializeInventoryUi()
    {
        _inventoryView.Initialize();

        foreach (SO_Equipment item in _initialInventory)
        {
            _inventoryView.AddItem(item);
        }

        foreach (SO_Equipment equipment in _initialEquippedItems.Values)
        {
            _inventoryView.AddEquipment(equipment);
        }
    }

    private void HandleItemSold(SO_Equipment soldItem)
    {
        _inventory.Remove(soldItem);
        _playerMoney.AddMoney(soldItem.sellingPrice);

        AudioManager.instance.Play(Sounds.ItemSell);
    }

    private void HandleItemPurchased(SO_Equipment newItem)
    {
        _inventory.Add(newItem);
        _playerMoney.SubtractMoney(newItem.purchasePrice);

        AudioManager.instance.Play(Sounds.ItemPurchase);
    }
}
