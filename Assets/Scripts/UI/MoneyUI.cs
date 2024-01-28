using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Player player;

    private void Start()
    {
        player.OnMoneyChanged += UpdateMoneyText;
        UpdateMoneyText(player.Money);
    }

    public void UpdateMoneyText(int amount)
    {
        moneyText.text = $"${amount}";
    }
}
