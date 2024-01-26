using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float Health { get; set; }
    Transform HealthbarTransform { get;}
    void TakeDamage(float amount, Player player);
    event Action<IDamageable> OnDestroyed;
    event Action<float, float> OnHealthChanged;

}
