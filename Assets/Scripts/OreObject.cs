using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreObject : MonoBehaviour, IDamageable, IDestructible
{
    [SerializeField]
    private float health;
    
    public event Action<IDamageable> OnDestroyed;

    public float Health
    {
        get => health;
        set => health = value;
    }
    public void TakeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            DestroyObject();
        }
    }
    

    public void DestroyObject()
    {
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
