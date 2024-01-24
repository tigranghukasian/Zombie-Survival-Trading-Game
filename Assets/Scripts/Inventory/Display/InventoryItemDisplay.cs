using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetSprite(Sprite _sprite)
    {
        image.sprite = _sprite;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }

    public void HideSprite()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }
}
