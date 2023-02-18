using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private PlayerMenu playerMenu;
    private InventoryController inventoryController;
    [SerializeField] public List<SO_Equipment> inventory = new List<SO_Equipment>();

    private void Awake()
    {
        playerMenu = UIManager.instance.playerMenu;
        inventoryController = UIManager.instance.inventoryController;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (playerMenu.gameObject.activeSelf)
            {
                playerMenu.ClosePlayerMenu();
            }
            else
            {
                playerMenu.OpenPlayerMenu();
            }
        }
    }
}
