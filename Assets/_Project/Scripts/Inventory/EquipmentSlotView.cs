using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotView : InventorySlotView
{
    public event Action<EquipmentSlotView> OnSlotSelected;

    [SerializeField] private SpriteRenderer _spriteToChange;
    [SerializeField] private SO_Equipment.EquipmentType _slotType;

    public void EquipItem()
    {
        _spriteToChange.sprite = GetItem().equipmentSprite;
    }

    public SO_Equipment.EquipmentType GetEquipmentType()
    {
        return _slotType;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        OnSlotSelected?.Invoke(this);
    }
}
