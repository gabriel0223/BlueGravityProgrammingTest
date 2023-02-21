using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Purchasable : MonoBehaviour
{
    public event Action<Purchasable> OnTryPurchase;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemPrice;
    [Space] 
    [SerializeField] private float _shakeStrength;
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _destroyAnimationDuration;

    private SO_Equipment _item;

    public void Initialize(SO_Equipment item)
    {
        _item = item;

        UpdatePurchasableUI();
    }

    private void UpdatePurchasableUI()
    {
        _itemIcon.sprite = _item.equipmentSprite;
        _itemName.SetText(_item.name);
        _itemPrice.SetText(_item.purchasePrice.ToString());

        float iconScale;
        Vector2 iconPosition;
        
        //this adjustment needed to be done hardcoded because of the way the art pack was exported :(
        switch (_item.equipmentType)
        {
            case SO_Equipment.EquipmentType.Top:
                iconScale = 3.3f;
                iconPosition = new Vector2(-272, 62);
                break;
            case SO_Equipment.EquipmentType.Head:
                iconScale = 2.6f;
                iconPosition = new Vector2(-272, -20);
                break;
            case SO_Equipment.EquipmentType.Bottom:
                iconScale = 3.3f;
                iconPosition = new Vector2(-272, 103);
                break;
            case SO_Equipment.EquipmentType.Face:
                iconScale = 3.3f;
                iconPosition = new Vector2(-275, 4);
                break;
            case SO_Equipment.EquipmentType.Weapon:
            default:
                iconScale = 3.7f;
                iconPosition = new Vector2(-316, 132);
                break;
        }
        
        _itemIcon.transform.localScale = Vector3.one * iconScale;
        _itemIcon.transform.localPosition = iconPosition;
    }

    public SO_Equipment GetItem()
    {
        return _item;
    }

    public void TryPurchase()
    {
        OnTryPurchase.Invoke(this);
    }

    public void PlayPurchaseErrorAnimation()
    {
        transform.DOShakePosition(_shakeDuration, _shakeStrength);
        AudioManager.instance.Play(Sounds.Error);
    }

    public void OnPurchaseSuccessful()
    {
        transform.DOScaleY(0, _destroyAnimationDuration).OnComplete(() => Destroy(gameObject));
    }
}
