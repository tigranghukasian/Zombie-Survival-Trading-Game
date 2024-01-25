using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    private float timeAlive = 0;
    private void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifeTime)
        {
            Destroy(gameObject);
        }
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        // if (other.TryGetComponent(out IHealth))
        // {
        //     
        // }
    }
}
