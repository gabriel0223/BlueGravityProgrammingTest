using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action<InventorySlotView> OnSlotSelected;

    [SerializeField] private Image _slotIcon;
    [SerializeField] private HoverGrowerButton _iconAnimation;

    private EquipmentData _item;
    private ItemInfoWindow _itemInfoWindow;
    
    protected void Awake()
    {
        UpdateItemIcon();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsOccupied())
        {
            return;
        }

        HoverSlot();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        OnSlotSelected?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Reset();
    }

    public virtual void AddItem(EquipmentData item, bool hoverAnimation)
    {
        _item = item;

        UpdateItemIcon();

        if (hoverAnimation)
        {
            HoverSlot();
        }
    }

    public void RemoveItem()
    {
        _item = null;

        _itemInfoWindow.Hide();
        UpdateItemIcon();
    }

    public void DeleteItem()
    {
        RemoveItem();
        UpdateItemIcon();
        _itemInfoWindow.Hide();
    }

    public void SetItemInfoWindow(ItemInfoWindow itemInfoWindow)
    {
        _itemInfoWindow = itemInfoWindow;
    }

    public bool IsOccupied()
    {
        return _item != null;
    }

    public EquipmentData GetItem()
    {
        return _item;
    }

    public Image GetIcon()
    {
        return _slotIcon;
    }

    public void Reset()
    {
        Debug.Log("chamei");

        if (!IsOccupied())
        {
            return;
        }
        
        _itemInfoWindow.Hide();
        _iconAnimation.Shrink();
    }

    private void UpdateItemIcon()
    {
        if (!IsOccupied())
        {
            _slotIcon.enabled = false;
            return;
        }

        _slotIcon.enabled = true;
        _slotIcon.sprite = _item.equipmentSprite;

         AdjustIconSize();
    }

    private void AdjustIconSize()
    {
        float iconScale;
        Vector2 iconPosition;
        
        //this adjustment needed to be done hardcoded because of the way the art pack was exported :( 
        switch (_item.equipmentType)
        {
            case EquipmentData.EquipmentType.Top:
                iconScale = 2.57f;
                iconPosition = new Vector2(3, 40);
                break;
            case EquipmentData.EquipmentType.Head:
                iconScale = 1.8f;
                iconPosition = new Vector2(5, -19);
                break;
            case EquipmentData.EquipmentType.Bottom:
                iconScale = 2.5f;
                iconPosition = new Vector2(5, 74);
                break;
            case EquipmentData.EquipmentType.Face:
                iconScale = 2.6f;
                iconPosition = new Vector2(1.8f, 5);
                break;
            case EquipmentData.EquipmentType.Weapon:
            default:
                iconScale = 3.2f;
                iconPosition = new Vector2(-39.5f, 111.5f);
                break;
        }

        _slotIcon.transform.localScale = Vector3.one * iconScale;
        _iconAnimation.SetOriginalScale(Vector3.one * iconScale);

        _slotIcon.transform.localPosition = iconPosition;
    }

    private void HoverSlot()
    {
        _iconAnimation.Grow();

        _itemInfoWindow.UpdateItemInfo(_item.name, _item.description, _item.sellingPrice);
        _itemInfoWindow.Show();
    }
}
