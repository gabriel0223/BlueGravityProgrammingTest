using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    private PlayerController playerControlller;
    private InventoryController inventoryController;

    private void Awake()
    {
        playerControlller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        inventoryController = UIManager.instance.inventoryController;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPlayerMenu()
    {
        if (UIManager.instance.interactingWithUI) return;
        
        UIManager.instance.interactingWithUI = true;
        UIManager.instance.uiState = UIManager.UIStates.Equipping;
        gameObject.SetActive(true);
        AudioManager.instance.Play("Pop01");
        playerControlller.LockInput(true);
    }
    
    public void ClosePlayerMenu()
    {
        if (inventoryController.isCarryingItem) return;
        
        AudioManager.instance.Play("ClickBack");
        playerControlller.LockInput(false);
        UIManager.instance.interactingWithUI = false;
        UIManager.instance.uiState = UIManager.UIStates.Idle;
        
        if (UIManager.instance.itemInfoWindow.activeSelf)
            UIManager.instance.itemInfoWindow.SetActive(false);
            
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
