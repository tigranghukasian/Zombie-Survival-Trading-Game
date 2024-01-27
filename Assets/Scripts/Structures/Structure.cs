using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Damageable
{
    
    public override void Kill()
    {
        base.Kill();
        Destroy(gameObject);
    }
}
