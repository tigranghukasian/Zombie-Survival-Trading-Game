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
    [SerializeField] private float damageOverlayFadeInOrOutDuration = 0.4f;
    [SerializeField] private float damageOverlayFadeOutStartAfter = 0.3f;

    private void Awake()
    {
        player.OnHealthChanged += UpdateHealth;
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        damageOverlay.FadeIn(damageOverlayFadeInOrOutDuration, 0,() =>
        {
            damageOverlay.FadeOut(damageOverlayFadeInOrOutDuration, damageOverlayFadeOutStartAfter);
        });
        healthAmountText.text = health.ToString();
        float percentage = health / maxHealth;
        healthFill.sizeDelta = new Vector2(healthFill.sizeDelta.x, percentage * healthFillFullSize);

    }
}
