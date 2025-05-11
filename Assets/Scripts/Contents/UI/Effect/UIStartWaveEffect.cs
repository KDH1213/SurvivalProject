using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStartWaveEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI spawnerCountText;

    private readonly string spawnerTextFormat = "몬스터 습격이 시작되었습니다.";

    [SerializeField]
    private Color startColor;

    [SerializeField]
    private Color targetColor;

    [SerializeField]
    private Vector3 maxScale;
    [SerializeField]
    private Vector3 minScale;

    [SerializeField]
    private Image backgroundImage;

    Sequence animationSequence;

    private void Awake()
    {
        animationSequence = DOTween.Sequence()
            .SetAutoKill(false)
            .Append(backgroundImage.transform.DOScale(maxScale, 0.5f))
            .AppendCallback(() => spawnerCountText.gameObject.SetActive(true))
            .AppendInterval(3f)
            .AppendCallback(() => spawnerCountText.gameObject.SetActive(false))
            .Append(backgroundImage.transform.DOScale(minScale, 0.5f))
            .AppendCallback(() => gameObject.SetActive(false));

        animationSequence.SetLoops(-1);
    }

    public void OnStrartEffect()
    {
        gameObject.SetActive(true);
    }


    private void OnDisable()
    {
        animationSequence.Pause();
        backgroundImage.transform.localScale = minScale;
        spawnerCountText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        animationSequence.Play();
    }
}