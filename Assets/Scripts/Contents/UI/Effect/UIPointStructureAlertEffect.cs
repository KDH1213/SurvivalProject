using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPointStructureAlertEffect : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup alertView;
    [SerializeField]
    private TextMeshProUGUI hpText;

    [SerializeField]
    private Image alertPanel;

    [SerializeField]
    private Color startColor;

    [SerializeField]
    private Color endColor;

    [SerializeField]
    private float appearTime;
    [SerializeField]
    private float lifeTime;

    [SerializeField]
    private float fadeTime = 0.5f;

    private float maxHp;

    private Coroutine alertEffectCoroutine = null;

    private readonly string hpFormat = "남은 체력 : {0}";
    private readonly string decimalPiontFormat = "f1";

    private float currentLifeTime;

    private void Awake()
    {
        var pointStructureStat = GameObject.FindWithTag(Tags.PointStructure).GetComponentInChildren<CharactorStats>();
        pointStructureStat.damegedEvent.AddListener(OnTakeDamaged);
        var hpStat = pointStructureStat.CurrentStatTable[StatType.HP];
        hpStat.OnChangeValue += OnChangeHp;
        maxHp = hpStat.MaxValue;
    }

    public void OnTakeDamaged()
    {
        if(alertEffectCoroutine == null)
        {
            alertEffectCoroutine = StartCoroutine(CoAlertEffect());
        }
        else
        {
            currentLifeTime = Time.time + lifeTime;
        }
    }

    public void OnChangeHp(float currentHp)
    {
        var currentPersent = (currentHp / maxHp) * 100f;
        hpText.text = string.Format(hpFormat, currentPersent.ToString(decimalPiontFormat));
    }

    private IEnumerator CoAlertEffect()
    {
        if(!alertView.gameObject.activeSelf)
        {
            alertView.gameObject.SetActive(true);
        }
        var waitForEndOfFrame = new WaitForEndOfFrame();

        alertView.alpha = 0f;
        float currentTime = 0f;
        while(currentTime < appearTime)
        {
            yield return waitForEndOfFrame;
            alertView.alpha = Mathf.Lerp(0f, 1f, currentTime / appearTime);
            currentTime += Time.deltaTime;
        }

        currentLifeTime = Time.time + lifeTime;

        bool isFadeIn = true;
        float currentFadeTime = 0f;

        while (currentLifeTime > Time.time)
        {
            yield return waitForEndOfFrame;

            if(isFadeIn)
            {
                alertPanel.color = Color.Lerp(startColor, endColor, currentFadeTime / fadeTime);
            }
            else
            {
                alertPanel.color = Color.Lerp(endColor, startColor, currentFadeTime / fadeTime);
            }

            currentFadeTime += Time.deltaTime;
            if (currentFadeTime > fadeTime)
            {
                isFadeIn = !isFadeIn;
                currentFadeTime = 0f;
            }
        }


        currentTime = 0f;
        while (currentTime < appearTime)
        {
            yield return waitForEndOfFrame;
            alertView.alpha = Mathf.Lerp(1f, 0f, currentTime / appearTime);
            currentTime += Time.deltaTime;
        }

        alertView.alpha = 0f;
        alertEffectCoroutine = null;
        alertPanel.color = startColor;
    }



}
