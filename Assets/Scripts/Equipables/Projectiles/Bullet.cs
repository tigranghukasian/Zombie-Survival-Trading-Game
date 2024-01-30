using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject bulletImpactPrefab;
    [SerializeField] private LayerMask layerMask;

    private float damage;
    private bool hasHit;
    private Collider collliderHit;

    public void Init(float _damage)
    {
        damage = _damage;
        Debug.DrawRay(transform.position, transform.forward * 0.2f, Color.blue, 5f);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position,  0.2f, layerMask);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && hitCollider.TryGetComponent(out Enemy enemy))
            {
                Hit(hitCollider);
            }
        }
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
            Destroy(gameObject);
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
        Instantiate(bulletImpactPrefab, other.ClosestPoint(transform.position), Quaternion.identity);
        Destroy(gameObject);
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage, null);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        //Hit(other);
    }
}
