using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using EquipmentType = SO_Equipment.EquipmentType;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlayerMenuView _playerMenuView;
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

        _inputManager.OnEscape += HandleEscapeInput;
        _inputManager.OnReturn += HandleReturnInput;

        _inventoryView.OnEquipItem += HandleItemEquipped;
    }

    private void OnDestroy()
    {
        _inputManager.OnEscape -= HandleEscapeInput;
        _inputManager.OnReturn -= HandleReturnInput;

        _inventoryView.OnEquipItem -= HandleItemEquipped;
    }

    private void Initialize()
    {
        _inventory = new List<SO_Equipment>(_initialInventory);
        _equippedItems = new Dictionary<EquipmentType, SO_Equipment>(_initialEquippedItems);

        InitializeInventoryUi();
    }

    private void HandleEscapeInput()
    {
        _playerMenuView.OpenPlayerMenu();
    }

    private void HandleReturnInput()
    {
        if (_playerMenuView.gameObject.activeSelf)
        {
            _playerMenuView.ClosePlayerMenu();
        }
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
}
