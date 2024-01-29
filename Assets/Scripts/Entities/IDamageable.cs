using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float MaxHealth { get; set; }
    float Health { get; set; }
    
    bool IsDead { get; set; }
    Transform HealthbarTransform { get; set; }
    void TakeDamage(float amount, IDamager damager);
    event Action<IDamageable> OnDestroyed;
    event Action<float, float> OnHealthChanged;
    event Action<float> OnDamaged;
    void Kill();

}
