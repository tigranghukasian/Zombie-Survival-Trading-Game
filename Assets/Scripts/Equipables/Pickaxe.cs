using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : ToolEquipable
{
    [SerializeField] private GameObject impactBloodPrefab;
    [SerializeField] private float damageValue;
    [SerializeField] private AudioClip hitSoundStone;
    [SerializeField] private AudioClip hitSoundFlesh;
    [SerializeField] private AudioClip missSound;
    
    public override void Use()
    {
    }
    
    public override void Fire()
    {
        base.Fire();
        bool hasHit = false;
        IDamageable damageable = null;
        for (int i = 0; i < ToolDetection.DamageablesInRange.Count; i++)
        {
            hasHit = true;
            damageable = ToolDetection.DamageablesInRange[i];
            if (damageable is OreObject or Enemy)
            {
                damageable.TakeDamage(damageValue, Owner);
            }
            break;
        }

        if (hasHit)
        {
            if (damageable is OreObject)
            {
                SoundManager.Instance.PlayAudioClip(hitSoundStone);
            }
            else if (damageable is Enemy)
            {
                Enemy enemy = (Enemy)damageable;
                Instantiate(impactBloodPrefab, enemy.transform.position, Quaternion.identity);
                SoundManager.Instance.PlayAudioClip(hitSoundFlesh);
            }
            else
            {
                SoundManager.Instance.PlayAudioClip(missSound);
            }
        }
        else
        {
            SoundManager.Instance.PlayAudioClip(missSound);
        }
    }
}
