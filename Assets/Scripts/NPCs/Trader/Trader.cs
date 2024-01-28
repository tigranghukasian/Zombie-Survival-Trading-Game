using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : Interactable
{
    [SerializeField] private string traderName;
    [SerializeField] private TradeUI tradeUI;
    [SerializeField] private List<TradeItem> tradeItems = new List<TradeItem>();

    public List<TradeItem> TradeItems => tradeItems;

    public override void Interact()
    {
        base.Interact();
        tradeUI.ShowTradeUI(this);
    }

    public override void UnInteract()
    {
        base.UnInteract();
        tradeUI.HideTraderUI();
    }
}
