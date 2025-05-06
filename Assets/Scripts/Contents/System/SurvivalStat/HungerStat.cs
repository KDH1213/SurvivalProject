using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HungerStat : SurvivalStatBehaviour
{
    public UnityEvent<bool> onExtraPenaltyEvent;

    public UnityEvent onHpPenaltyEvnet;

    [SerializeField]
    [Range(0f, 1f)]
    private float startPenaltyPersent = 0.5f;

    [SerializeField]
    private float valueDownTime = 0.5f;
    [SerializeField]
    private float hpDownTime = 5f;

    [SerializeField]
    private Slider hungerSlider;

    [SerializeField]
    private TextMeshProUGUI hungerPersentText;

    [SerializeField]
    private Sprite debuffIcon; 
    [SerializeField]
    private Sprite extraDebuffIcon;


    private readonly string persentFormat = "{0}%";

    private bool isHpDown;
    private float currentTime = 0f;
    private float hungerSkillValue = 0f;
    private float totalValueDownTime = 0f;
    private bool isOnExtraPenalty = false;


    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Hunger;
        value = MaxValue;
        totalValueDownTime = valueDownTime;
        Load();

        OnChangeValue();

        var uIDebuffIcon = GameObject.FindWithTag(Tags.UIDebuffIcon);
        if(uIDebuffIcon != null)
        {
            var debuff = uIDebuffIcon.GetComponent<UIDebuffIconView>().CreateDebuffIcon(debuffIcon, "느려짐", "이동속도 감소");
            onStartPenaltyEvent.AddListener((_) => { debuff.gameObject.SetActive(true); });
            onEndPenaltyEvent.AddListener((_) => { debuff.gameObject.SetActive(false); });
            debuff.gameObject.SetActive(false);

            var newdebuff = uIDebuffIcon.GetComponent<UIDebuffIconView>().CreateDebuffIcon(extraDebuffIcon, "체력 감소", "체력 떨어짐");
            onExtraPenaltyEvent.AddListener((active) => { newdebuff.gameObject.SetActive(active); });
            newdebuff.gameObject.SetActive(false);
        }
    }

    //private void Start()
    //{
    //    var uIDebuffIcon = GameObject.Find(Tags.UIDebuffIcon);
    //    if (uIDebuffIcon != null)
    //    {
    //        var debuff = uIDebuffIcon.GetComponent<UIDebuffIconView>().CreateDebuffIcon(debuffIcon, "느려짐", "이동속도 감소");
    //        onStartPenaltyEvent.AddListener((_) => { debuff.gameObject.SetActive(true); });
    //        onEndPenaltyEvent.AddListener((_) => { debuff.gameObject.SetActive(false); });

    //        debuff = uIDebuffIcon.GetComponent<UIDebuffIconView>().CreateDebuffIcon(extraDebuffIcon, "체력 감소", "체력 떨어짐");
    //        onExtraPenaltyEvent.AddListener((active) => { debuff.gameObject.SetActive(active); });
    //    }
    //}

    private void Update()
    {
        currentTime += Time.deltaTime;

        if(isOnDebuff)
        {
            if (value <= 0f)
            {
                CheckHpDown();
            }
            else
            {
                CheckValueDown();
            }
        }
        else
        {
            CheckValueDown();
            if (!isOnDebuff && IsActivationCheckPenalty())
            {
                OnStartPenalty();
            }
        }
    }

    private void CheckValueDown()
    {
        if (currentTime >= totalValueDownTime)
        {
            currentTime -= totalValueDownTime;
            value -= MaxValue * 0.01f;
            this.value = Mathf.Max(value, 0f);

            if(value == 0f)
            {
                isOnExtraPenalty = true;
                onExtraPenaltyEvent?.Invoke(isOnExtraPenalty);
            }
           OnChangeValue();
        }
    }

    private void CheckHpDown()
    {
        if (currentTime >= hpDownTime)
        {
            currentTime -= hpDownTime;
            onHpPenaltyEvnet?.Invoke();
        }
    }

    protected override bool IsActivationCheckPenalty()
    {
        return value < MaxValue * startPenaltyPersent;
    }

    public override void AddPenaltyValue(float value)
    {
        this.value += value;

        this.value = Mathf.Clamp(this.value, 0f, MaxValue);
        OnChangeValue();

        if (!isOnDebuff && IsActivationCheckPenalty())
        {
            OnStartPenalty();
        }
        else if (isOnDebuff)
        {
            isOnDebuff = IsActivationCheckPenalty();
            
            if (value > 0f)
            {
                isOnExtraPenalty = false;
                onExtraPenaltyEvent?.Invoke(isOnExtraPenalty);
            }


            if (!isOnDebuff)
            {
                OnStopPenalty();
            }
        }
    }

    public override void SubPenaltyValue(float value)
    {
        this.value -= value;

        this.value = Mathf.Clamp(this.value, 0f, MaxValue);
        OnChangeValue();

        if (!isOnDebuff && IsActivationCheckPenalty())
        {
            OnStartPenalty();
        }
        else if (isOnDebuff)
        {
            isOnDebuff = IsActivationCheckPenalty();

            if (value > 0f)
            {
                isOnExtraPenalty = false;
                onExtraPenaltyEvent?.Invoke(isOnExtraPenalty);
            }

            if (!isOnDebuff)
            {
                OnStopPenalty();
            }
        }
    }

    public void OnSetHungerSkillValue(float value)
    {
        hungerSkillValue = value;
        totalValueDownTime = valueDownTime + hungerSkillValue;
    }

    public override void OnStartPenalty()
    {
        base.OnStartPenalty();
    }

    public override void OnStopPenalty()
    {
        base.OnStopPenalty();
    }

    public void OnChangeValue()
    {
        var persent = value / MaxValue;
        hungerSlider.value = persent;
        hungerPersentText.text = string.Format(persentFormat, ((int)(persent * 100f)).ToString());
    }
}
