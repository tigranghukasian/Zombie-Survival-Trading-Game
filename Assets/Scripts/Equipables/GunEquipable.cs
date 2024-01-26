using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEquipable : Equipable
{
    public override void OnEquip()
    {
        
    }

    public override void Use()
    {
        timePassed += Time.deltaTime;
        if (timePassed > fireRate)
        {
            Fire();
            timePassed = 0;
        }
    }

    public override void Fire()
    {
        
    }
}
