using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlayerMenuView _playerMenuView;
    [SerializeField] private InventoryView _inventoryView;
    [Space]
    [SerializeField] private SO_Equipment _currentFaceEquipment;
    [SerializeField] private SO_Equipment _currentHeadEquipment;
    [SerializeField] private SO_Equipment _currentTopEquipment;
    [SerializeField] private SO_Equipment _currentBottomEquipment;
    [SerializeField] private SO_Equipment _currentWeaponEquipment;

    [SerializeField] public List<SO_Equipment> _inventory = new List<SO_Equipment>();

    private bool _hasInitializedUi;

    private void Start()
    {
        _inputManager.OnEscape += HandleEscapeInput;
        _inputManager.OnReturn += HandleReturnInput;
    }

    private void OnDestroy()
    {
        _inputManager.OnEscape -= HandleEscapeInput;
        _inputManager.OnReturn -= HandleReturnInput;
    }

    private void HandleEscapeInput()
    {
        _playerMenuView.OpenPlayerMenu();

        if (_hasInitializedUi)
        {
            return;
        }

        InitializeInventoryUi();
    }

    private void HandleReturnInput()
    {
        if (_playerMenuView.gameObject.activeSelf)
        {
            _playerMenuView.ClosePlayerMenu();
        }
    }

    private void InitializeInventoryUi()
    {
        _inventoryView.InitializeInventory(_inventory);

        _inventoryView.InitializeEquipment(_currentFaceEquipment);
        _inventoryView.InitializeEquipment(_currentHeadEquipment);
        _inventoryView.InitializeEquipment(_currentTopEquipment);
        _inventoryView.InitializeEquipment(_currentBottomEquipment);
        _inventoryView.InitializeEquipment(_currentWeaponEquipment);

        _hasInitializedUi = true;
    }
}
