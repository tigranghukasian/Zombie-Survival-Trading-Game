using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IEquipable
{
    
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePos;
    [SerializeField] private float range = 100f;
    
    [SerializeField]
    private float fireRate;

    public float FireRate
    {
        get => fireRate;
        set => fireRate = value;
    }
    
    private float timePassed;
    public void Use()
    {
        timePassed += Time.deltaTime;
        if (timePassed > fireRate)
        {
            Fire();
            timePassed = 0;
        }
    }
    
    public void Fire()
    {
        var bulletObject = Instantiate(bullet,firePos.transform.position, transform.rotation);
    }
}
