using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public int playerCoins;
    private TextMeshProUGUI playerCoinsText;

    // Start is called before the first frame update
    void Start()
    {
        playerCoinsText = UIManager.instance.playerMoneyWindow.GetComponentInChildren<TextMeshProUGUI>();
        playerCoinsText.SetText(playerCoins.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCoins(int amount)
    {
        playerCoins += amount;
        playerCoinsText.SetText(playerCoins.ToString());
    }
}
