using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private GameObject purchasablePrefab;
    [SerializeField] private Transform itemsOnSale;
    [SerializeField] private GameObject soldOutText;
    private InteractiveShop activeInteractiveShop;
    private PlayerMoneyController playerMoney;

    private List<SO_Equipment> _itemsForSale;

    void Start()
    {
        playerMoney = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoneyController>();
    }

    public void Initialize(InteractiveShop shop)
    {
        activeInteractiveShop = shop;
        _itemsForSale = shop.GetItems();

        foreach (var item in _itemsForSale)
        {
            var newPurchasable = Instantiate(purchasablePrefab, itemsOnSale).GetComponent<Purchasable>();
            newPurchasable.item = item;
            newPurchasable.UpdatePurchasableUI();
        }
    }

    public void AddItemToShop(SO_Equipment item)
    {
        activeInteractiveShop._items.Add(item);
        
        var newPurchasable = Instantiate(purchasablePrefab, itemsOnSale).GetComponent<Purchasable>();
        newPurchasable.item = item;
        newPurchasable.UpdatePurchasableUI();
    }
    
    public void RemoveItemFromShop(SO_Equipment item)
    {
        activeInteractiveShop._items.Remove(item);
    }

    public void SellItem(SO_Equipment item)
    {
        AddItemToShop(item);
        playerMoney.AddMoney(item.sellingPrice);
        AudioManager.instance.Play(Sounds.ItemSell);
    }
}
