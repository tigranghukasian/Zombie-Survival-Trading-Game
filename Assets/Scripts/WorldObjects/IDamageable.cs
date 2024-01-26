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
    void TakeDamage(float amount, Player player);
    event Action<IDamageable> OnDestroyed;
    event Action<float, float> OnHealthChanged;
    void Kill();

}
