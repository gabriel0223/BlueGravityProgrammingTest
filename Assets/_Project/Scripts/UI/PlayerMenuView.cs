using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuView : MonoBehaviour
{
    public event Action OnOpenPlayerMenu;
    public event Action OnClosePlayerMenu;

    [SerializeField] private InventoryView _inventoryView;

    public void OpenPlayerMenu()
    {
        gameObject.SetActive(true);
        AudioManager.instance.Play(Sounds.Pop01);
        
        OnOpenPlayerMenu?.Invoke();
    }
    
    public void ClosePlayerMenu()
    {
        if (_inventoryView.IsDraggingItem)
        {
            return;
        }

        _inventoryView.OnClose();
        AudioManager.instance.Play(Sounds.ClickBack);

        gameObject.SetActive(false);

        OnClosePlayerMenu?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
