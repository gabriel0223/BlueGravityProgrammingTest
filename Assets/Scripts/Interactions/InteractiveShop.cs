using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveShop : MonoBehaviour, IInteractive
{
    private ShopController shopController;
    
    [Header("list of items being sold in this shop")]
    public List<SO_Equipment> items;

    private void Awake()
    {
        shopController = UIManager.instance.shopController;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (UIManager.instance.interactingWithUI) return;
        
        shopController.OpenShop(this, items);
    }
}
