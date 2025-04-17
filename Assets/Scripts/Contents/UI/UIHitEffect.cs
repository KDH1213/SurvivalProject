using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHitEffect : MonoBehaviour
{
    [SerializeField]
    private float hitEffectTime = 0.5f;

    [SerializeField]
    private GameObject targetObject;
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;
    [SerializeField]
    private Image colorImage;

    private float currentTime = 0f;

    public void OnStartEffect()
    {
        currentTime = 0f;
        colorImage.color = startColor;
        targetObject.SetActive(true);
    }

    public void OnEndEffect()
    {
        targetObject.SetActive(false);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        colorImage.color = Color.Lerp(startColor, endColor, currentTime / hitEffectTime);

        if (currentTime >= hitEffectTime)
        {
            gameObject.SetActive(false);
            OnEndEffect();
        }
    }
}
