using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject peacefulTimeParent;
    [SerializeField] private GameObject spawnTimeTextParent;
    [SerializeField] private Slider peacefulTimeSlider;
    [SerializeField] private Slider spawnTimeSlider;

    [SerializeField] private WaveSpawner waveSpawner;
    private WaveState currentWaveState;

     private void OnEnable()
     {
         waveSpawner.OnStateChanged += UpdateUI;
         IntroUI.OnIntroFinished += Activate;
     }
    
     private void OnDisable()
     {
         waveSpawner.OnStateChanged -= UpdateUI;
     }

     private void Update()
     {
         if (currentWaveState != null)
         {
             float percentage = currentWaveState.Timer / currentWaveState.Duration;
             peacefulTimeSlider.value = (1 - percentage);
             spawnTimeSlider.value = (1 - percentage);
         }
         
     }

     private void Activate()
     {
         peacefulTimeParent.GetComponent<UIFader>().FadeIn(0.8f, null);
     }

     private void UpdateUI(WaveState newState)
     {
         currentWaveState = newState;
         peacefulTimeParent.SetActive(newState is WavePeacefulState);
         spawnTimeTextParent.SetActive(newState is WaveSpawnState);
         

     }
}
