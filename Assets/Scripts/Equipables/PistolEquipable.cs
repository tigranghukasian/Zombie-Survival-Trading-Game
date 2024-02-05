using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PistolEquipable : Equipable
{
    public Transform PlayerTransform { get; set; }
    [SerializeField] private Item bulletItem;
    [SerializeField] private Transform firePos;
    [SerializeField] private float range = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject trailPrefab;
    [SerializeField] private MuzzleFlash muzzleFlash;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip noAmmoSound;
    

    public Inventory Inventory { get; set; }

    private bool fireCooldownPassed;

    public override void OnEquip()
    {
        PoolManager.Instance.CreatePool(bulletPrefab);
        PoolManager.Instance.CreatePool(trailPrefab);
    }

    public override void Use()
    {
        fireCoolDown += Time.deltaTime;
        if (fireCoolDown > fireRate)
        {
            fireCooldownPassed = true;
        }
    }

    protected void Setup(AudioClip _fireSound)
    {
        fireSound = _fireSound;
    }

    public override void Fire()
    {
        if (fireCooldownPassed)
        {
            bool hasBullet = Inventory.RemoveItem(bulletItem, 1);
            if (hasBullet)
            {
                var bulletObject = PoolManager.Instance.GetPooledObject(bulletPrefab, firePos.position, PlayerTransform.rotation );
                Bullet bullet = bulletObject.GetComponent<Bullet>();
                var trailObject = PoolManager.Instance.GetPooledObject(trailPrefab, firePos.position, PlayerTransform.rotation );
                BulletTrail trail = trailObject.GetComponent<BulletTrail>();
                bullet.Init(damage,trail);
                trail.LifeTime = bullet.LifeTime;
                trail.bulletTransform = bulletObject.transform;
                muzzleFlash.Play();
                SoundManager.Instance.PlayAudioClip(fireSound);
            }
            else
            {
                SoundManager.Instance.PlayAudioClip(noAmmoSound);
            }
            fireCoolDown = 0;
        }

    }
}
