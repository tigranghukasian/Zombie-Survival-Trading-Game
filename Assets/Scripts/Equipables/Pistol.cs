using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : PistolEquipable
{
    
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePos;
    [SerializeField] private float range = 100f;
    [SerializeField] private AudioClip fireSound;

    private bool canFire;

    public override void OnEquip()
    {
        canFire = true;
    }
    public override void Use()
    {
        
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > fireRate)
        {
            canFire = true;
        }
    }

    public override void Fire()
    {
        if (canFire)
        {
            SoundManager.Instance.PlayAudioClip(fireSound);
            var bulletObject = Instantiate(bullet,firePos.transform.position, PlayerTransform.rotation);
            canFire = false;
            timePassed = 0;
        }
        
        
    }
}
