using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trade Item", menuName = "TradeSystem/TradeItem")]
public class TradeItem : ScriptableObject
{
    public Item item;
    public int amountSelling;

    public int CalculatePrice()
    {
        return item.price * amountSelling;
    }
}
