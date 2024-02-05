using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour, IPoolable
{
    public int PoolableId { get; set; }
    public GameObject GameObject { get; set; }

    public Transform bulletTransform { get; set; }
    public float LifeTime { get; set; }
    

    [SerializeField] private float gunTrailDestroyAfter;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }
    

    public void OnPoolableObjectGet()
    {
        if (trailRenderer != null)
        {
            trailRenderer.Clear();
            trailRenderer.enabled = true;
        }
    }
    

    public void OnPoolableObjectRelease()
    {
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (bulletTransform != null)
        {
            transform.position = bulletTransform.position;
        }
        else
        {
            PoolManager.Instance.ReleasePooledObject(this);
        }
    }

    // IEnumerator DestroyAfter(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     Destroy(gameObject);
    // }

}
