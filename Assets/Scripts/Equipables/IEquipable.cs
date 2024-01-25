using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  IEquipable
{
    public float FireRate { get; set; }
    
    void Use();

    void Fire();
}
