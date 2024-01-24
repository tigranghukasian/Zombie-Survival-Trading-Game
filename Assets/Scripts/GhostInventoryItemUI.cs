using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GhostInventoryItemUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountText;

    public void SetSprite(Sprite sprite, int amount)
    {
        itemImage.sprite = sprite;
        amountText.text = amount == 0? "": amount.ToString();
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
