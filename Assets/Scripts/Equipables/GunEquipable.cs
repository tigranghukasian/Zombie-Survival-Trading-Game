using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEquipable : Equipable
{
    [SerializeField] private AudioClip gunshotSound;
    public override void OnEquip()
    {
        
    }

    public override void Use()
    {
        fireCoolDown += Time.deltaTime;
        if (fireCoolDown > fireRate)
        {
            Fire();
            fireCoolDown = 0;
        }
    }

    public override void Fire()
    {
        SoundManager.Instance.PlayAudioClip(gunshotSound);
    }
}
