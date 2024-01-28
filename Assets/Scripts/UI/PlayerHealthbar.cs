using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthAmountText;
    [SerializeField] private RectTransform healthFill;
    [SerializeField] private float healthFillFullSize;
    [SerializeField] private Player player;
    [SerializeField] private UIFader damageOverlay;
    [SerializeField] private float damageOverlayFadeHalfDuration = 0.4f;

    private void Awake()
    {
        player.OnHealthChanged += UpdateHealth;
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        damageOverlay.FadeIn(damageOverlayFadeHalfDuration, () =>
        {
            damageOverlay.FadeOut(damageOverlayFadeHalfDuration, null);
        });
        healthAmountText.text = health.ToString();
        float percentage = health / maxHealth;
        healthFill.sizeDelta = new Vector2(healthFill.sizeDelta.x, percentage * healthFillFullSize);

    }
}
