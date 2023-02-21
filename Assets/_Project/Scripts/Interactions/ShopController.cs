using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public static event Action<SO_Equipment> OnPlayerBuyItem;

    [SerializeField] private Purchasable _purchasablePrefab;
    [SerializeField] private Transform _purchasableScrollList;

    private List<SO_Equipment> _itemsForSale = new List<SO_Equipment>();
    private InteractiveShop _interactiveShop;
    private PlayerMoneyController _playerMoney;
    private InventoryView _playerInventoryWindow;

    private void OnDestroy()
    {
        _interactiveShop.SetItems(_itemsForSale);

        _playerInventoryWindow.OnPlayerSellItem -= AddItemToShop;
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

    public void SetInventoryWindow(InventoryView inventoryWindow)
    {
        _playerInventoryWindow = inventoryWindow;

        _playerInventoryWindow.OnPlayerSellItem += AddItemToShop;
    }

    private void AddItemToShop(SO_Equipment item)
    {
        _itemsForSale.Add(item);

        var newPurchasable = Instantiate(_purchasablePrefab, _purchasableScrollList);
        newPurchasable.Initialize(item);

        newPurchasable.OnTryPurchase += HandleTryPurchase;
    }

    private void HandleTryPurchase(Purchasable purchasable)
    {
        SO_Equipment item = purchasable.GetItem();

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
        SO_Equipment item = purchasable.GetItem();

        _itemsForSale.Remove(item);
        purchasable.OnPurchaseSuccessful();

        purchasable.OnTryPurchase -= HandleTryPurchase;

        OnPlayerBuyItem?.Invoke(item);
    }
}
