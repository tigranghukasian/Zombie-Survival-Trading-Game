using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureGhost : MonoBehaviour
{
    private List<Collider> collidersInRange = new List<Collider>();

    public List<Collider> CollidersInRange => collidersInRange;

    private void OnTriggerEnter(Collider other)
    {
        collidersInRange.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        collidersInRange.Remove(other);
    }
    
}
