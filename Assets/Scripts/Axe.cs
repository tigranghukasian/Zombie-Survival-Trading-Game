using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour, IEquipable
{
    [SerializeField]private ToolDetection toolDetection;
    [SerializeField] private float damageValue;
    
    public void Fire()
    {
        Debug.Log("Axe Fire" + toolDetection.DamageablesInRange.Count );
        for (int i = 0; i < toolDetection.DamageablesInRange.Count; i++)
        {
            var damageable = toolDetection.DamageablesInRange[i];
            if (damageable is TreeObject)
            {
                damageable.TakeDamage(damageValue);
                Debug.Log("TAKE DAMAGE");
            }
        }
    }
}
