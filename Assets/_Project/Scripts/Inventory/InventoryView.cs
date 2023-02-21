using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public event Action<SO_Equipment> OnEquipItem;

    [SerializeField] private Transform _itemSelector;
    [SerializeField] private ItemInfoWindow _itemInfoWindow;
    [SerializeField] private InventorySlotView[] _inventorySlots;
    [SerializeField] private EquipmentSlotView[] _equipmentSlots;
    
    private bool _isDraggingItem;
    private SO_Equipment _currentSelectedItem;
    private readonly Vector3 _dragItemPositionOffset = new Vector2(30, -30);

    public bool IsDraggingItem => _isDraggingItem;

    void Update()
    {
        _itemSelector.position = Mouse.current.position.ReadValue();
    }

    public void Initialize()
    {
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

    public void AddItem(SO_Equipment equipment)
    {
        InventorySlotView emptySlot = GetFirstEmptySlot();

        emptySlot.AddItem(equipment, false);
    }

    public void AddEquipment(SO_Equipment equipment)
    {
        EquipmentSlotView equipmentSlot = _equipmentSlots.First(slot => slot.GetEquipmentType() == equipment.equipmentType);

        equipmentSlot.AddItem(equipment, false);
    }

    public void OnClose()
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
        // if (UIManager.instance.uiState == UIManager.UIStates.Shopping)
        // {
        //     UIManager.instance.shopController.SellItem(slotView.item);
        //     slotView.DeleteItem();
        //     return;
        // }
        
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

        OnEquipItem?.Invoke(slot.GetItem());
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
        SO_Equipment secondSlotItem = slot.GetItem();
            
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
