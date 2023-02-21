using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuView : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;

    public void OpenPlayerMenu()
    {
        gameObject.SetActive(true);
        AudioManager.instance.Play(Sounds.Pop01);
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
    }
}
