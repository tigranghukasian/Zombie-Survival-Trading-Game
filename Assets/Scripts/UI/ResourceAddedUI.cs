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
        GetComponent<UIFader>().FadeOut(fadeTime,timeUntilFade, OnFadeCompleted);
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
