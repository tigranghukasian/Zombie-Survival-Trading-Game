using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TradeUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI speechText;
    [SerializeField] private float fadeDuration = 0.3f;
    
    [SerializeField] private Transform tradeItemUiParent;
    [SerializeField] private GameObject tradeItemUiPrefab;
    [SerializeField] private InventoryHolder inventoryHolder;

    private ItemDatabase itemDatabase;
    private InventorySlot sellSlot;
    

    private List<TradeItemUI> tradeItemUis = new List<TradeItemUI>();

    private UIFader fader;

    private void Awake()
    {
        fader = GetComponent<UIFader>();
    }

    private void Start()
    {
        sellSlot = inventoryHolder.Inventory.GetSlot(0);
        itemDatabase = inventoryHolder.Inventory.ItemDatabase;
    }


    public void ShowTradeUI(Trader trader)
    {
        gameObject.SetActive(true);
        fader.FadeIn(fadeDuration);
        for (int i = 0; i < trader.TradeItems.Count; i++)
        {
            TradeItemUI tradeItemUi = Instantiate(tradeItemUiPrefab, tradeItemUiParent).GetComponent<TradeItemUI>();
            tradeItemUi.Init(trader.TradeItems[i], OnBuyButtonClicked);
            tradeItemUis.Add(tradeItemUi);
        }
    }

    private void OnBuyButtonClicked(TradeItem tradeItem)
    {
        var amountToSpend = tradeItem.CalculatePrice();
        if (player.HasMoney(amountToSpend))
        {
            player.DecreaseMoney(amountToSpend);
            player.PlayerInventoryHolder.Inventory.AddItem(tradeItem.item, tradeItem.amountSelling);
        }
    }

    public void OnSellButtonClicked()
    {
        var item = itemDatabase.GetItem(sellSlot.Id);
        var amount = sellSlot.Amount;
        if (item != null)
        {
            var moneyToGain = item.price * amount;
            player.IncreaseMoney(moneyToGain);
        }
        sellSlot.UpdateSlot(-1, 0);
    }
    public void HideTraderUI()
    {
        for (int i = 0; i < tradeItemUis.Count; i++)
        {
            Destroy(tradeItemUis[i].gameObject);
        }
        tradeItemUis.Clear();
        fader.FadeOut(fadeDuration,0, () =>
        {
            gameObject.SetActive(false);
        });
    }
}
