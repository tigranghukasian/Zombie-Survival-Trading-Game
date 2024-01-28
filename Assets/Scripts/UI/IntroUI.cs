using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroUI : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public string textToType;
    public float typingSpeed = 0.1f;
    public float initialDelay = 0.5f;
    public float delayAfterTyping = 1f;
    private UIFader fader;
    public static Action OnIntroFinished;


    private void Start()
    {
        introText.text = "";
        StartCoroutine(TypeText());
        fader = GetComponent<UIFader>();
        fader.FadeIn(0, null);
       
    }

    IEnumerator TypeText()
    {
        yield return new WaitForSeconds(initialDelay);
        foreach (char letter in textToType.ToCharArray())
        {
            introText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(delayAfterTyping);
        fader.FadeOut(3, () =>
        {
            OnIntroFinished?.Invoke();
            gameObject.SetActive(false);
        });
    }
}
