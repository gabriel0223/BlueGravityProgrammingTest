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
    [SerializeField] private EquipmentData.EquipmentType _slotType;

    public override void AddItem(EquipmentData item, bool hoverAnimation)
    {
        base.AddItem(item, hoverAnimation);

        EquipItem();
    }

    public void EquipItem()
    {
        _spriteToChange.sprite = GetItem().equipmentSprite;
    }

    public EquipmentData.EquipmentType GetEquipmentType()
    {
        return _slotType;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        OnSlotSelected?.Invoke(this);
    }
}
