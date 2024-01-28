using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeItemUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    private Action<TradeItem> onBuyButtonClicked;
    private TradeItem tradeItem;
    public void Init(TradeItem _tradeItem, Action<TradeItem> _onBuyButtonClicked)
    {
        tradeItem = _tradeItem;
        itemImage.sprite = tradeItem.item.sprite;
        itemName.text = tradeItem.item.name;
        amountText.text = $"x{tradeItem.amountSelling.ToString()}";
        priceText.text = $"${tradeItem.item.price.ToString()}";
        onBuyButtonClicked = _onBuyButtonClicked;
        buyButton.onClick.AddListener(BuyButtonClicked);
    }

    private void BuyButtonClicked()
    {
        onBuyButtonClicked?.Invoke(tradeItem);
    }
}
