using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public Transform itemSelector;
    private GameObject itemInfoWindow;
    [HideInInspector] public SO_Equipment currentSelectedItem;
    [SerializeField] private GameObject inventorySlots;
    
    [HideInInspector] public bool isCarryingItem;
    private Vector3 itemPositionOffset = new Vector2(30, -30);

    private void Awake()
    {
        itemInfoWindow = UIManager.instance.itemInfoWindow;
    }

    // Start is called before the first frame update
    void Start()
    {
        var playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory;
        InitializeInventoryUI(playerInventory);
    }

    // Update is called once per frame
    void Update()
    {
        itemSelector.position = Input.mousePosition;
    }

    public void InitializeInventoryUI(List<SO_Equipment> inventory)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            var slot = inventorySlots.transform.GetChild(i).GetComponent<InventorySlot>();
            slot.AddItem(inventory[i], false);
        }
    }

    public void SelectSlot(InventorySlot slot)
    {
        if (UIManager.instance.uiState == UIManager.UIStates.Shopping)
        {
            UIManager.instance.shopController.SellItem(slot.item);
            slot.DeleteItem();
            return;
        }
        
        if (!isCarryingItem) //if it's not selecting the second slot
        {
            currentSelectedItem = slot.item;
            
            slot.RemoveItem();
            itemInfoWindow.SetActive(false);
            var iconCopy = Instantiate(slot.slotIcon, itemSelector); //copy icon to be carried on item selector
            slot.slotIcon.enabled = false; //disable icon on the slot

            iconCopy.GetComponent<RectTransform>().localPosition += itemPositionOffset;
            
            isCarryingItem = true;
            AudioManager.instance.Play("SelectItem");
        }
        else
        {
            AddItemToSlot(slot, currentSelectedItem);
        }
    }
    
    public void SelectSlot(EquipmentSlot slot)
    {
        if (!isCarryingItem || slot.item.equipmentType != currentSelectedItem.equipmentType) return;
        AddItemToSlot(slot, slot.item);
        slot.EquipItem();
    }

    private void AddItemToSlot(InventorySlot slot, SO_Equipment item)
    {
        if (slot.item == null) //if there's no item in the slot
        {
            slot.AddItem(item, true);

            Destroy(itemSelector.GetChild(0).gameObject); //destroy icon copy in the item selector
            isCarryingItem = false;
        }
        else
        {
            ReplaceItem();
        }

        void ReplaceItem()
        {
            var secondSlotItem = slot.item;
            
            //PUT NEW ICON IN THE ITEM SELECTOR
            Destroy(itemSelector.GetChild(0).gameObject); //destroy icon copy in the item selector
            var iconCopy = Instantiate(slot.slotIcon, itemSelector); //copy icon to be carried on item selector
            iconCopy.GetComponent<RectTransform>().localPosition += itemPositionOffset;
            
            //PUT OLD ITEM IN THE SECOND SLOT
            slot.AddItem(currentSelectedItem, true);

            currentSelectedItem = secondSlotItem;
        }
        
        AudioManager.instance.Play("DeselectItem");
    }

    public InventorySlot GetFirstEmptySlot()
    {
        return inventorySlots.GetComponentsInChildren<InventorySlot>().First(s => !s.full);
    }
}
