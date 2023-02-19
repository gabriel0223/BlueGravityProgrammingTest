using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPumpkin : MonoBehaviour
{
    public void GiveMoneyToPlayer(int quantity)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoney>().UpdateCoins(quantity);
    }
}
