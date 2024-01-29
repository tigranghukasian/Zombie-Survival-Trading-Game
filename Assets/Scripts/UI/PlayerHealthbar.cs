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
        player.OnHealthChanged += HealthChanged;
        player.OnDamaged += d => Damaged();
    }

    public void HealthChanged(float health, float maxHealth)
    {
        
        healthAmountText.text = health.ToString();
        float percentage = health / maxHealth;
        healthFill.sizeDelta = new Vector2(healthFill.sizeDelta.x, percentage * healthFillFullSize);

    }

    public void Damaged()
    {
        damageOverlay.FadeIn(damageOverlayFadeInOrOutDuration, 0,() =>
        {
            damageOverlay.FadeOut(damageOverlayFadeInOrOutDuration, damageOverlayFadeOutStartAfter);
        });
    }
}
