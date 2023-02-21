using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public event Action<EquipmentData> OnPlayerEquipItem;
    public event Action<EquipmentData> OnPlayerSellItem;

    [SerializeField] private Transform _itemSelector;
    [SerializeField] private ItemInfoWindow _itemInfoWindow;
    [SerializeField] private InventorySlotView[] _inventorySlots;
    [SerializeField] private EquipmentSlotView[] _equipmentSlots;

    private InventoryMode _inventoryMode;
    private bool _isDraggingItem;
    private EquipmentData _currentSelectedItem;
    private readonly Vector3 _dragItemPositionOffset = new Vector2(30, -30);

    public bool IsDraggingItem => _isDraggingItem;

    void Update()
    {
        _itemSelector.position = Mouse.current.position.ReadValue();
    }

    public void Initialize()
    {
        ShopView.OnPlayerBuyItem += AddItem;

        foreach (InventorySlotView slot in _inventorySlots)
        {
            slot.SetItemInfoWindow(_itemInfoWindow);
            slot.OnSlotSelected += SelectInventorySlot;
        }

        foreach (EquipmentSlotView slot in _equipmentSlots)
        {
            slot.SetItemInfoWindow(_itemInfoWindow);
            slot.OnSlotSelected += SelectEquipmentSlot;
        }
    }

    private void OnDisable()
    {
        OnClose();
    }

    private void OnDestroy()
    {
        ShopView.OnPlayerBuyItem -= AddItem;
    }

    public void AddItem(EquipmentData equipmentData)
    {
        InventorySlotView emptySlot = GetFirstEmptySlot();

        emptySlot.AddItem(equipmentData, false);
    }

    public void AddEquipment(EquipmentData equipmentData)
    {
        EquipmentSlotView equipmentSlot = _equipmentSlots.First(slot => slot.GetEquipmentType() == equipmentData.equipmentType);

        equipmentSlot.AddItem(equipmentData, false);
    }

    public void SetInventoryMode(InventoryMode newInventoryMode)
    {
        _inventoryMode = newInventoryMode;
    }

    private void OnClose()
    {
        foreach (InventorySlotView slot in _inventorySlots)
        {
            slot.Reset();
        }

        foreach (EquipmentSlotView slot in _equipmentSlots)
        {
            slot.Reset();
        }
    }

    private void SelectInventorySlot(InventorySlotView slot)
    {
        if (_inventoryMode == InventoryMode.Shopping)
        {
            SellItemFromSlot(slot);

            return;
        }

        if (_isDraggingItem)
        {
            DetachItemFromMouse();

            if (!slot.IsOccupied())
            {
                slot.AddItem(_currentSelectedItem, true);
            }
            else
            {
                ReplaceItemInSlot(slot);
            }

            AudioManager.instance.Play(Sounds.DeselectItem);
        }
        else
        {
            if (!slot.IsOccupied())
            {
                return;
            }

            GrabItemFromSlot(slot);
        }
    }

    private void SellItemFromSlot(InventorySlotView slot)
    {
        EquipmentData itemToBeSold = slot.GetItem();

        slot.RemoveItem();
        OnPlayerSellItem?.Invoke(itemToBeSold);
    }

    private void SelectEquipmentSlot(EquipmentSlotView slot)
    {
        if (!_isDraggingItem || slot.GetItem().equipmentType != _currentSelectedItem.equipmentType)
        {
            return;
        }

        DetachItemFromMouse();

        if (!slot.IsOccupied())
        {
            slot.AddItem(_currentSelectedItem, true);
        }
        else
        {
            ReplaceItemInSlot(slot);
        }

        AudioManager.instance.Play(Sounds.DeselectItem);

        OnPlayerEquipItem?.Invoke(slot.GetItem());
    }

    private void GrabItemFromSlot(InventorySlotView slot)
    {
        _currentSelectedItem = slot.GetItem();

        slot.RemoveItem();
        AttachItemToMouse(slot);
        
        AudioManager.instance.Play(Sounds.SelectItem);
    }

    private void AttachItemToMouse(InventorySlotView slot)
    {
        //copy item icon to be dragged on item selector
        Image itemIcon = Instantiate(slot.GetIcon(), _itemSelector);
        itemIcon.transform.localPosition += _dragItemPositionOffset;
        itemIcon.transform.SetAsFirstSibling();
        itemIcon.enabled = true;

        _isDraggingItem = true;
    }

    private void DetachItemFromMouse()
    {
        //destroy icon copy in the item selector
        Destroy(_itemSelector.GetChild(0).gameObject);

        _isDraggingItem = false;
    }

    private void ReplaceItemInSlot(InventorySlotView slot)
    {
        EquipmentData secondSlotItem = slot.GetItem();
            
        AttachItemToMouse(slot);

        //put old item in the second slot
        slot.AddItem(_currentSelectedItem, true);

        _currentSelectedItem = secondSlotItem;
    }

    public InventorySlotView GetFirstEmptySlot()
    {
        return _inventorySlots.First(s => !s.IsOccupied());
    }
}
