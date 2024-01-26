using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ResourceObject : MonoBehaviour, IDamageable, IDestructible
{
    [SerializeField]
    private float health;

    [SerializeField] private Item resourceItem;
    [SerializeField] private int amountToGive;
    
    public event Action<IDamageable> OnDestroyed;
    public event Action<float, float> OnHealthChanged;
    
    [SerializeField] private Transform healthBarTransform;
    private float fullHealth;
    public Transform HealthbarTransform
    {
        get => healthBarTransform;
    }

    public float Health
    {
        get => health;
        set => health = value;
    }
    private void Awake()
    {
        fullHealth = health;
    }
    public void TakeDamage(float amount, Player Owner)
    {
        Health -= amount;
        GameUIManager.Instance.ShowHealthbar(this);
        OnHealthChanged?.Invoke(Health, fullHealth);

        if (Health <= 0)
        {
            Owner.InventoryHolder.Inventory.AddItem(resourceItem,amountToGive );
            GameUIManager.Instance.AddResourceAddedUI(resourceItem, amountToGive);
            DestroyObject();
            return;
        }
        TriggerDamageEffect();
    }
    
    private void TriggerDamageEffect()
    {
        float duration = 0.3f;
        float bobbingAmount = 0.2f;

        // Animate the object to move up and then back down
        transform.DOJump(transform.position, bobbingAmount, 1, duration);
    }
    

    public void DestroyObject()
    {
        DOTween.Kill(this);
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
