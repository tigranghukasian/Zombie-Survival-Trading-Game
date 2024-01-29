using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : Interactable
{
    [SerializeField] private string traderName;
    [SerializeField] private string traderTitle;
    [TextArea(10,20)]
    [SerializeField] private string traderIntroductionText;
    [SerializeField] private TradeUI tradeUI;
    [SerializeField] private List<TradeItem> tradeItems = new List<TradeItem>();
    [SerializeField] private AudioClip speechClip;

    public AudioClip SpeechClip => speechClip;

    public string TraderName => traderName;
    public string TraderTitle => traderTitle;
    public string TraderIntroductionText => traderIntroductionText;

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
