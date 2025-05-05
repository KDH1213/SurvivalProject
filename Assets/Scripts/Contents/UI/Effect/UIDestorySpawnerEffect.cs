using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDestorySpawnerEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI spawnerCountText;

    private readonly string spawnerTextFormat = "몬스터 거점이 파괴되었습니다.\n 남은 몬스터 거점 : {0}/{1}";

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

    public void OnStrartEffect(int count, int maxCount)
    {
        gameObject.SetActive(true);

        if(count == maxCount)
        {
            spawnerCountText.text = "모든 몬스터 거점이 파괴되었습니다.";
        }
        else
        {
            spawnerCountText.text = string.Format(spawnerTextFormat, maxCount - count, maxCount);
        }
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
