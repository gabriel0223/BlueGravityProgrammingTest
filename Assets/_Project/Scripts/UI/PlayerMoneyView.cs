using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyView : MonoBehaviour
{
    [SerializeField] private PlayerMoneyController _playerMoney;
    [SerializeField] private TMP_Text _moneyText;

    private void Start()
    {
        SetMoneyText(_playerMoney.Money);

        _playerMoney.OnMoneyUpdated += HandleMoneyUpdated;
    }

    private void OnDestroy()
    {
        _playerMoney.OnMoneyUpdated -= HandleMoneyUpdated;
    }

    private void SetMoneyText(int money)
    {
        _moneyText.SetText(money.ToString());
    }

    private void HandleMoneyUpdated(int newMoney)
    {
        SetMoneyText(newMoney);
    }
}
