using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameClearMessageView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gameClearText;

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
            .AppendCallback(() => gameClearText.gameObject.SetActive(true))
            .AppendInterval(3f)
            .AppendCallback(() => gameClearText.gameObject.SetActive(false))
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
        gameClearText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        animationSequence.Play();
    }
}
