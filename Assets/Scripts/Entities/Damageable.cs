using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour, IDamageable
{
    public bool IsDead { get; set; }
    [field: SerializeField] public Transform HealthbarTransform { get; set; }
    public float Health { get; set; } = 100;
    [field: SerializeField] public float MaxHealth { get; set; } = 100;
    
    
    public event Action<IDamageable> OnDestroyed;
    public event Action<float, float> OnHealthChanged;
    public event Action<float> OnDamaged;

    private void Awake()
    {
       
    }

    public virtual void OnSpawn()
    {
        if (HealthbarTransform == null)
        {
            HealthbarTransform = transform;
        }
        Health = MaxHealth;
        IsDead = false;
    }
    public virtual void TakeDamage(float amount, IDamager Damager)
    {
        Health -= amount;
        GameUIManager.Instance.ShowHealthbar(this);
        OnHealthChanged?.Invoke(Health, MaxHealth);

        if (Health <= 0)
        {
            IsDead = true;
            Kill();
        }
    }

    public virtual void Kill()
    {
        OnDestroyed?.Invoke(this);
    }
}
