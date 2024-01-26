using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    [SerializeField] private GameObject light;
    
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out ParticleSystem ps))
            {
                particleSystems.Add(ps);
            }
        }
        light.SetActive(false);
    }
    

    public void Play()
    {
        for (int i = 0; i < particleSystems.Count; i++)
        {
            particleSystems[i].Stop();
            particleSystems[i].Play();
            
        }
        light.SetActive(true);
    }
}

