using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject bulletImpactPrefab;

    private float damage;
    private bool hasHit;
    private Collider collliderHit;

    public void Init(float _damage)
    {
        damage = _damage;
    }

    private float timeAlive = 0;
    private void FixedUpdate()
    {
        if (hasHit)
        {
            Hit(collliderHit);
        }
        timeAlive += Time.fixedDeltaTime;
        float step = speed * Time.fixedDeltaTime;
        if (timeAlive >= lifeTime)
        {
            Destroy(gameObject);
        }
        Vector3 newPosition = transform.position + transform.forward * step;
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, step))
        {
            hasHit = true;
            newPosition = hit.point;
            collliderHit = hit.collider;
        }
        transform.position = newPosition;
    }

    private void Hit(Collider other)
    {
        Instantiate(bulletImpactPrefab, other.ClosestPoint(transform.position), Quaternion.identity);
        Destroy(gameObject);
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(10, null);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        //Hit(other);
    }
}
