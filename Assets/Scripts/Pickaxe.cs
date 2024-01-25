using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : ToolEquipable
{
    
    [SerializeField] private float damageValue;

    public override void Use()
    {
    }
    
    public override void Fire()
    {
        Debug.Log("FIRE PICKAXE");
        for (int i = 0; i < ToolDetection.DamageablesInRange.Count; i++)
        {
            var damageable = ToolDetection.DamageablesInRange[i];
            if (damageable is OreObject)
            {
                damageable.TakeDamage(damageValue);
            }
        }
    }
}
