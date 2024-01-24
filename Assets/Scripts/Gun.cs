using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IEquipable
{
    [SerializeField] private Transform firePos;
    [SerializeField] private float range = 100f;
    
    public void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePos.position, transform.forward, out hit, range))
        {
            
        }
    }
}
