using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyController : MonoBehaviour
{
    public event Action<int> OnMoneyUpdated;

    [SerializeField] private int _initialMoney;

    private int _money;

    public int Money => _money;

    // Start is called before the first frame update
    void Start()
    {
        _money = _initialMoney;
    }

    public void AddMoney(int amount)
    {
        _money += amount;

        OnMoneyUpdated?.Invoke(_money);
    }

    public void SubtractMoney(int amount)
    {
        _money -= amount;

        OnMoneyUpdated?.Invoke(_money);
    }
}
