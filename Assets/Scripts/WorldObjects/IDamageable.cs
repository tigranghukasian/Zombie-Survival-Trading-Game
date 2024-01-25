using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float Health { get; set; }
    Transform HealthbarTransform { get;}
    void TakeDamage(float amount);
    event Action<IDamageable> OnDestroyed;
    event Action<float, float> OnHealthChanged;

}
