using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class UIManager : MonoBehaviour
{
    public enum UIStates
    {
        Idle, Talking, Equipping, Shopping 
    }
    
    [Header("REFERENCES")]
    public PlayerMenu playerMenu;
    public ShopController shopController;
    public InventoryController inventoryController;
    public GameObject inventoryWindow;
    public GameObject itemInfoWindow;
    public GameObject playerMoneyWindow;
    public static UIManager instance;

    [HideInInspector] public bool interactingWithUI;
    [HideInInspector] public UIStates uiState;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
