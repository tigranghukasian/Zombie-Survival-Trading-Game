using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int amount;

    public Item Item => item;
    public int Amount => amount;
}
