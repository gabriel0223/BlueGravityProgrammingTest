using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : InventorySlot
{
    [SerializeField] private SpriteRenderer spriteToChange;
    public SO_Equipment.EquipmentType slotType;
    
    private void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void EquipItem()
    {
        spriteToChange.sprite = item.equipmentSprite;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        inventoryController.SelectSlot(this);
    }
}
