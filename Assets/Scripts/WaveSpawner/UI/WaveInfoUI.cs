using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject peacefulTimeParent;
    [SerializeField] private GameObject spawnTimeTextParent;
    [SerializeField] private Slider peacefulTimeSlider;
    [SerializeField] private Slider spawnTimeSlider;
    [SerializeField] private TextMeshProUGUI waveText;


    private CanvasGroup waveTextCanvasGroup;

    [SerializeField] private WaveSpawner waveSpawner;
    private WaveState currentWaveState;

     private void OnEnable()
     {
         waveSpawner.OnStateChanged += UpdateUI;
         IntroUI.OnIntroFinished += Activate;
         
         peacefulTimeParent.GetComponent<UIFader>().FadeOut(0);
         waveText.GetComponent<UIFader>().FadeOut(0);
         waveTextCanvasGroup = waveText.GetComponent<CanvasGroup>();
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
             waveText.text = $"Wave {waveSpawner.GetCycle() + 1}";
         }
         
     }

     private void Activate()
     {
         peacefulTimeParent.GetComponent<UIFader>().FadeIn(1f);
         waveText.GetComponent<UIFader>().FadeIn(1f,1f);
     }

     private void UpdateUI(WaveState newState)
     {
         currentWaveState = newState;
         peacefulTimeParent.SetActive(newState is WavePeacefulState);
         spawnTimeTextParent.SetActive(newState is WaveSpawnState);
         waveTextCanvasGroup.alpha = (newState is WaveSpawnState)?1:0;


     }
}
