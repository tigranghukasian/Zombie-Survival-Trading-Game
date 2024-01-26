using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIFader))]
public class ResourceAddedUI : MonoBehaviour
{
    [SerializeField] private Image resourceImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private float timeUntilFade;
    [SerializeField] private float fadeTime;
    [SerializeField] private float floatSpeed;
    

    public void Init(Sprite resourceSprite, int amount)
    {
        resourceImage.sprite = resourceSprite;
        amountText.text = $"+ {amount.ToString()}";
        StartCoroutine(CallFadeAfterDelay(timeUntilFade));
    }

    IEnumerator CallFadeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<UIFader>().FadeOut(fadeTime, OnFadeCompleted);
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    public void OnFadeCompleted()
    {
        Destroy(gameObject);
    }
}
