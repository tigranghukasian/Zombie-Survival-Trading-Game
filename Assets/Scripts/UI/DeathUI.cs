using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI youSurvivedText;
    [SerializeField] private UIFader screenFader;

    [SerializeField] private UIFader restartButtonFader;
    private float timeSurvived;

    private void Start()
    {
        player.OnDeath += () =>
        {
            Show(timeSurvived);
        };
        screenFader.gameObject.SetActive(false);
    }

    private void Update()
    {
        timeSurvived += Time.deltaTime;
    }

    public void Show(float timeSurvived)
    {
        screenFader.gameObject.SetActive(true);
        screenFader.SetAlpha(0);

        var youSurvivedFader = youSurvivedText.GetComponent<UIFader>();
        youSurvivedFader.SetAlpha(0);
        restartButtonFader.SetAlpha(0);
        
        int hours = (int)timeSurvived / 3600;
        int minutes = (int)(timeSurvived % 3600) / 60;
        int seconds = (int)timeSurvived % 60;
        string timeString = "";
        if (hours > 0)
        {
            timeString += $"{hours} hours ";
        }
        if (minutes > 0)
        {
            timeString += $"{minutes} minutes ";
        }
        if (seconds > 0)
        {
            timeString += $"{seconds} seconds";
        }
        timeString = timeString.Trim();
        youSurvivedText.text = $"You survived: {timeString}";
        
        screenFader.FadeIn(1f, 1f,() =>
        {
            youSurvivedText.GetComponent<UIFader>().FadeIn(0.5f,0, () =>
            {
                restartButtonFader.FadeIn(0.5f);
            });
        });
       
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
