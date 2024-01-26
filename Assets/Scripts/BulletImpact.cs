using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    [SerializeField] private float destructAfter;
    private float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > destructAfter)
        {
            Destroy(gameObject);
        }
    }
}
