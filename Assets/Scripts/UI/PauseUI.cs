using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private UIFader pauseMenuFader;
    private bool isPaused;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }

    private void Pause()
    {
        pauseMenuFader.SetAlpha(0);
        pauseMenuFader.gameObject.SetActive(true);
        pauseMenuFader.FadeIn(0.3f, 0, () =>
        {
            Time.timeScale = 0;
        });
    }

    private void UnPause()
    {
        Time.timeScale = 1;
        pauseMenuFader.FadeOut(0.3f, 0, () =>
        {
            pauseMenuFader.gameObject.SetActive(false);
        });
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}
