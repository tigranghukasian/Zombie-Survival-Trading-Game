using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemObject itemObject;

    public ItemObject ItemObject => itemObject;
}
