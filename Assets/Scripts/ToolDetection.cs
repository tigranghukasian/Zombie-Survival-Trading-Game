using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDetection : MonoBehaviour
{
    private List<IDamageable> damageablesInRange = new List<IDamageable>();

    public List<IDamageable> DamageablesInRange => damageablesInRange;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageablesInRange.Add(damageable);
            damageable.OnDestroyed += RemoveDamageable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageablesInRange.Remove(damageable);
            damageable.OnDestroyed -= RemoveDamageable;
        }
    }
    
    private void RemoveDamageable(IDamageable damageable)
    {
        damageable.OnDestroyed -= RemoveDamageable;
        DamageablesInRange.Remove(damageable);
    }

}
