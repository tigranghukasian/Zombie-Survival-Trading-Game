using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    public int PoolableId { get; set; }
    public GameObject GameObject { get; set; }


    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject bulletImpactSparksPrefab;
    [SerializeField] private GameObject bulletImpactBloodPrefab;
    [SerializeField] private LayerMask layerMask;

    private float damage;
    private bool hasHit;
    private Collider collliderHit;
    private BulletTrail trail;

    public float LifeTime => lifeTime;

    public void Init(float _damage, BulletTrail _trail)
    {
        damage = _damage;
        trail = _trail;
        Debug.DrawRay(transform.position, transform.forward * 0.2f, Color.blue, 5f);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position,  0.2f, layerMask);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && hitCollider.TryGetComponent(out Enemy enemy))
            {
                Hit(hitCollider);
            }
        }

        PoolManager.Instance.CreatePool(bulletImpactSparksPrefab);
        PoolManager.Instance.CreatePool(bulletImpactBloodPrefab);
    }
    
    public void OnPoolableObjectGet()
    {
        
    }

    public void OnPoolableObjectRelease()
    {
        hasHit = false;
        timeAlive = 0;
    }

    private float timeAlive = 0;
    private void Update()
    {
        if (hasHit)
        {
            Hit(collliderHit);
            return;
        }
        hasHit = false;
        timeAlive += Time.deltaTime;
        float step = speed * Time.deltaTime;
        if (timeAlive >= lifeTime)
        {
            PoolManager.Instance.ReleasePooledObject(trail);
            PoolManager.Instance.ReleasePooledObject(this);
        }
        Vector3 newPosition = transform.position + transform.forward * step;
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,  step, layerMask))
        {
            hasHit = true;
            newPosition = hit.point;
            collliderHit = hit.collider;
        }
        transform.position = newPosition;
    }
    
    private void CheckIfHit() {}

    private void Hit(Collider other)
    {
        var impactPoint = other.ClosestPoint(transform.position);

        PoolManager.Instance.ReleasePooledObject(trail);
        PoolManager.Instance.ReleasePooledObject(this);
        
        if (other.TryGetComponent(out Enemy enemy))
        {
            var bulletImpactBlood = PoolManager.Instance.GetPooledObject(bulletImpactBloodPrefab,
                other.ClosestPoint(transform.position),  Quaternion.identity);
            enemy.TakeDamage(damage, null);
        }
        else
        {
            var bulletImpactSparks = PoolManager.Instance.GetPooledObject(bulletImpactSparksPrefab,
                other.ClosestPoint(transform.position),  Quaternion.identity);
        }
    }


}
