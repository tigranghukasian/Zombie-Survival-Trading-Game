using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;

    public void SetAmount(int _amount)
    {
        amountText.text = _amount <= 1? "": _amount.ToString();
    }
}
