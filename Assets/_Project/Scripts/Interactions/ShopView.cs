using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    public static event Action<EquipmentData> OnPlayerBuyItem;

    [SerializeField] private Purchasable _purchasablePrefab;
    [SerializeField] private Transform _purchasableScrollList;

    private List<EquipmentData> _itemsForSale = new List<EquipmentData>();
    private InteractiveShop _interactiveShop;
    private PlayerMoneyController _playerMoney;
    private InventoryView _playerInventoryWindow;

    private void OnEnable()
    {
        InventoryController.OnPlayerSellItem += AddItemToShop;
    }

    private void OnDestroy()
    {
        _interactiveShop.SetItems(_itemsForSale);

        InventoryController.OnPlayerSellItem -= AddItemToShop;
    }

    public void Initialize(InteractiveShop shop)
    {
        _interactiveShop = shop;

        foreach (var item in shop.GetItems())
        {
            AddItemToShop(item);
        }
    }

    public void SetPlayerMoney(PlayerMoneyController playerMoney)
    {
        _playerMoney = playerMoney;
    }

    private void AddItemToShop(EquipmentData item)
    {
        _itemsForSale.Add(item);

        var newPurchasable = Instantiate(_purchasablePrefab, _purchasableScrollList);
        newPurchasable.Initialize(item);

        newPurchasable.OnTryPurchase += HandleTryPurchase;
    }

    private void HandleTryPurchase(Purchasable purchasable)
    {
        EquipmentData item = purchasable.GetItem();

        if (_playerMoney.Money < item.purchasePrice)
        {
            purchasable.PlayPurchaseErrorAnimation();
        }
        else
        {
            PurchaseItem(purchasable);
        }
    }

    private void PurchaseItem(Purchasable purchasable)
    {
        EquipmentData item = purchasable.GetItem();

        _itemsForSale.Remove(item);
        purchasable.OnPurchaseSuccessful();

        purchasable.OnTryPurchase -= HandleTryPurchase;

        OnPlayerBuyItem?.Invoke(item);
    }
}
