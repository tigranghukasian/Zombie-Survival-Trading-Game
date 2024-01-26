using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : ToolEquipable
{
    [SerializeField] private float damageValue;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip missSound;

    public override void Use()
    {
        
    }
    
    public override void Fire()
    {
        bool hasHit = false;
        for (int i = 0; i < ToolDetection.DamageablesInRange.Count; i++)
        {
            var damageable = ToolDetection.DamageablesInRange[i];
            if (damageable is TreeObject or Enemy)
            {
                hasHit = true;
                damageable.TakeDamage(damageValue, Owner);
            }
        }
        if (hasHit)
        {
            SoundManager.Instance.PlayAudioClip(hitSound);
        }
        else
        {
            SoundManager.Instance.PlayAudioClip(missSound);
        }
    }

}
