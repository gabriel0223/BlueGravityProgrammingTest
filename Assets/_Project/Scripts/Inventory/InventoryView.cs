using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Transform _itemSelector;
    [SerializeField] private ItemInfoWindow _itemInfoWindow;
    [SerializeField] private InventorySlotView[] _inventorySlots;
    [SerializeField] private EquipmentSlotView[] _equipmentSlots;
    
    private bool _isCarryingItem;
    private SO_Equipment _currentSelectedItem;
    private readonly Vector3 _itemPositionOffset = new Vector2(30, -30);

    public bool IsCarryingItem => _isCarryingItem;

    void Start()
    {
        InitializeSlots();
    }

    // Update is called once per frame
    void Update()
    {
        _itemSelector.position = Mouse.current.position.ReadValue();
    }

    private void InitializeSlots()
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

    public void InitializeInventory(List<SO_Equipment> inventory)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            _inventorySlots[i].AddItem(inventory[i], false);
        }
    }

    public void InitializeEquipment(SO_Equipment equipment)
    {
        EquipmentSlotView equipmentSlot = _equipmentSlots.First(slot => slot.GetEquipmentType() == equipment.equipmentType);

        equipmentSlot.AddItem(equipment, false);
    }

    private void SelectInventorySlot(InventorySlotView slot)
    {
        // if (UIManager.instance.uiState == UIManager.UIStates.Shopping)
        // {
        //     UIManager.instance.shopController.SellItem(slotView.item);
        //     slotView.DeleteItem();
        //     return;
        // }
        
        if (!_isCarryingItem) //if it's not selecting the second slot
        {
            _currentSelectedItem = slot.GetItem();
            
            slot.RemoveItem();
            _itemInfoWindow.gameObject.SetActive(false);
            var iconCopy = Instantiate(slot.GetIcon(), _itemSelector); //copy icon to be carried on item selector
            slot.GetIcon().enabled = false; //disable icon on the slot

            iconCopy.GetComponent<RectTransform>().localPosition += _itemPositionOffset;
            
            _isCarryingItem = true;
            AudioManager.instance.Play("SelectItem");
        }
        else
        {
            AddItemToSlot(slot, _currentSelectedItem);
        }
    }

    private void SelectEquipmentSlot(EquipmentSlotView slotView)
    {
        if (!_isCarryingItem || slotView.GetItem().equipmentType != _currentSelectedItem.equipmentType) return;
        AddItemToSlot(slotView, slotView.GetItem());
        slotView.EquipItem();
    }

    private void AddItemToSlot(InventorySlotView slotView, SO_Equipment item)
    {
        if (slotView.GetItem() == null) //if there's no item in the slot
        {
            slotView.AddItem(item, true);

            Destroy(_itemSelector.GetChild(0).gameObject); //destroy icon copy in the item selector
            _isCarryingItem = false;
        }
        else
        {
            ReplaceItem();
        }

        void ReplaceItem()
        {
            var secondSlotItem = slotView.GetItem();
            
            //PUT NEW ICON IN THE ITEM SELECTOR
            Destroy(_itemSelector.GetChild(0).gameObject); //destroy icon copy in the item selector
            var iconCopy = Instantiate(slotView.GetIcon(), _itemSelector); //copy icon to be carried on item selector
            iconCopy.GetComponent<RectTransform>().localPosition += _itemPositionOffset;
            
            //PUT OLD ITEM IN THE SECOND SLOT
            slotView.AddItem(_currentSelectedItem, true);

            _currentSelectedItem = secondSlotItem;
        }
        
        AudioManager.instance.Play("DeselectItem");
    }

    public InventorySlotView GetFirstEmptySlot()
    {
        return _inventorySlots.First(s => !s.IsOccupied());
    }
}
