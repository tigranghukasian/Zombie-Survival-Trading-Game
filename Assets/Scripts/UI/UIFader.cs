using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeOut(float duration, float startAfter = 0, Action onFadeCompleted = null)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(startAfter,1, 0, duration, onFadeCompleted));
    }

    public void FadeIn(float duration, float startAfter = 0, Action onFadeCompleted = null)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(startAfter,0, 1, duration, onFadeCompleted));
    }

    IEnumerator Fade(float startAfter, float alphaStart, float alphaEnd, float duration, Action onFadeCompleted)
    {
        yield return new WaitForSeconds(startAfter);
        float time = 0;
        canvasGroup.alpha = alphaStart;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(alphaStart, alphaEnd, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = alphaEnd;

        if (onFadeCompleted != null)
        {
            onFadeCompleted();
        }
    }
}
