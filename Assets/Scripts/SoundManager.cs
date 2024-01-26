using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    private AudioSource audioSource;

    protected override void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayAudioClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
