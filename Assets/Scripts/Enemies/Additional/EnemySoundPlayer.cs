using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<SoundTypeList> soundTypeLists = new List<SoundTypeList>();
    
        
    private Coroutine soundCoroutine;
    public enum SoundType
    {
        Idle,
        Chase,
        Attack,
        Death,
    }

    public void PlayRandomTypeSoundOneShot(SoundType type)
    {
        if (soundCoroutine != null) 
        {
            StopCoroutine(soundCoroutine);
        }

        PlayRandomTypeSound(type);
    }
    
    private void PlayRandomTypeSound(SoundType type)
    {
        Debug.Log("PLAY SOUND OF TYPE " + type);
        SoundTypeList soundTypeList = soundTypeLists.FirstOrDefault(x => x.SoundType == type);
        if (soundTypeList == null)
        {
            Debug.Log("Can't find sounds for this type");
            return;
        }
        List<AudioClip> clips = soundTypeList.AudioClips;
        AudioClip clip = clips[Random.Range(0, clips.Count)];
        audioSource.PlayOneShot(clip);
    }


    public void StartPlayingContinuousRandomSounds(SoundType type, float minDelay, float maxDelay)
    {
        if (soundCoroutine != null) 
        {
            StopCoroutine(soundCoroutine);
        }
        soundCoroutine = StartCoroutine(PlayRandomSoundsContinuously(type, minDelay, maxDelay));
    }

    public void StopPlayingContinuousSounds()
    {
        if (soundCoroutine != null)
        {
            StopCoroutine(soundCoroutine);
            soundCoroutine = null;
        }
    }

    private IEnumerator PlayRandomSoundsContinuously(SoundType type, float minDelay, float maxDelay)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            PlayRandomTypeSound(type);
        }
    }

}

[System.Serializable]
public class SoundTypeList
{
    [field: SerializeField] public EnemySoundPlayer.SoundType SoundType { get; set; }
    [field: SerializeField] public List<AudioClip> AudioClips { get; set; }
}
