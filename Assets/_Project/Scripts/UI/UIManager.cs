using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : MonoBehaviour
{
    public event Action OnOpenMenu;
    public event Action OnCloseMenu;

    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlayerMoneyController _playerMoney;
    [SerializeField] private Transform _canvas;
    [SerializeField] private ShopView _shopViewPrefab;
    [Space]
    [SerializeField] private InventoryView _inventoryWindow;
    [SerializeField] private GameObject _equipmentWindow;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _closeButton;

    private ShopView _shopWindow;
    private bool _playerIsBrowingMenu;

    private void Start()
    {
        _inputManager.OnEscape += HandleEscapeInput;
        _inputManager.OnReturn += HandleReturnInput;

        InteractiveShop.OnShoppingStart += OpenShopWindow;
    }

    private void OnDestroy()
    {
        _inputManager.OnEscape -= HandleEscapeInput;
        _inputManager.OnReturn -= HandleReturnInput;

        InteractiveShop.OnShoppingStart -= OpenShopWindow;
    }

    private void OpenCustomizationMenu()
    {
        SetInventoryActive(true);
        _inventoryWindow.SetInventoryMode(InventoryMode.Normal);

        SetEquipmentWindowActive(true);
        SetQuitButtonActive(true);
        SetCloseButtonActive(true);

        AudioManager.instance.Play(Sounds.Pop01);
        _closeButton.onClick.AddListener(CloseCustomizationMenu);

        _playerIsBrowingMenu = true;
        OnOpenMenu?.Invoke();
    }

    private void CloseCustomizationMenu()
    {
        if (_inventoryWindow.IsDraggingItem)
        {
            return;
        }

        SetInventoryActive(false);
        SetEquipmentWindowActive(false);
        SetQuitButtonActive(false);
        SetCloseButtonActive(false);

        AudioManager.instance.Play(Sounds.ClickBack);
        _closeButton.onClick.RemoveAllListeners();

        _playerIsBrowingMenu = false;
        OnCloseMenu?.Invoke();
    }

    private void OpenShopWindow(InteractiveShop shop)
    {
        _shopWindow = Instantiate(_shopViewPrefab, _canvas);
        _shopWindow.Initialize(shop);
        _shopWindow.SetPlayerMoney(_playerMoney);
        _shopWindow.SetInventoryWindow(_inventoryWindow);

        SetInventoryActive(true);
        _inventoryWindow.SetInventoryMode(InventoryMode.Shopping);

        SetCloseButtonActive(true);

        AudioManager.instance.Play(Sounds.Pop01);
        _closeButton.onClick.AddListener(CloseShopWindow);

        _playerIsBrowingMenu = true;
        OnOpenMenu?.Invoke();
    }

    private void CloseShopWindow()
    {
        Destroy(_shopWindow.gameObject);

        SetInventoryActive(false);
        SetCloseButtonActive(false);

        AudioManager.instance.Play(Sounds.ClickBack);
        _closeButton.onClick.RemoveAllListeners();

        _playerIsBrowingMenu = false;
        OnCloseMenu?.Invoke();
    }

    private void HandleEscapeInput()
    {
        OpenCustomizationMenu();
    }

    private void HandleReturnInput()
    {
        if (!_playerIsBrowingMenu)
        {
            return;
        }

        if (_shopWindow != null)
        {
            CloseShopWindow();
        }
        else
        {
            CloseCustomizationMenu();
        }
    }

    private void SetInventoryActive(bool value)
    {
        _inventoryWindow.gameObject.SetActive(value);
    }

    private void SetEquipmentWindowActive(bool value)
    {
        _equipmentWindow.SetActive(value);
    }

    private void SetQuitButtonActive(bool value)
    {
        _quitButton.gameObject.SetActive(value);
    }

    private void SetCloseButtonActive(bool value)
    {
        _closeButton.gameObject.SetActive(value);
    }
}
