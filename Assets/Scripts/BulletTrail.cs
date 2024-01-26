using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    public Transform bulletTransform { get; set; }

    [SerializeField] private float gunTrailDestroyAfter;
    
    void Update()
    {
        if (bulletTransform != null)
        {
            transform.position = bulletTransform.position;
        }
        else
        {
            StartCoroutine(DestroyAfter(gunTrailDestroyAfter));
        }
    }

    IEnumerator DestroyAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
