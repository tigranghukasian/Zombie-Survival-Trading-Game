using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IEquipable
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePos;
    [SerializeField] private float range = 100f;
    
    public void Fire()
    {
        var bulletObject = Instantiate(bullet,firePos.transform.position, transform.rotation);
    }
}
