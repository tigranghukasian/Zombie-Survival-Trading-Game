using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipable : MonoBehaviour, IEquipable
{
    [SerializeField]
    protected float fireRate;

    public float FireRate
    {
        get => fireRate;
        set => fireRate = value;
    }
    public Player Owner { get; set; }
    
    protected float fireCoolDown;
    public abstract void OnEquip();
    public abstract void Use();

    public abstract void Fire();
    
}
