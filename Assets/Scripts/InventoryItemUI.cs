using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;

    public void SetSprite(Sprite sprite, bool visible)
    {
        itemImage.sprite = sprite;
        itemImage.color = visible? Color.white : Color.clear;
    }
}
