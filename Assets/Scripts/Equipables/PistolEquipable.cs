using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolEquipable : Equipable
{
    public Transform PlayerTransform { get; set; }
    [SerializeField] private Item bulletItem;
    [SerializeField] private Transform firePos;
    [SerializeField] private float range = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject trailPrefab;
    [SerializeField] private MuzzleFlash muzzleFlash;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip noAmmoSound;

    public Inventory Inventory { get; set; }

    private bool fireCooldownPassed;

    public override void OnEquip()
    {
        
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
                var bulletObject = Instantiate(bullet,firePos.transform.position, PlayerTransform.rotation).GetComponent<Bullet>();
                bulletObject.Init(damage);
                var trailObject = Instantiate(trailPrefab,firePos.position, PlayerTransform.rotation);
                BulletTrail trail = trailObject.GetComponent<BulletTrail>();
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
