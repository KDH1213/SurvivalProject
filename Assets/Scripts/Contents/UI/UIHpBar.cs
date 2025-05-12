using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UIHpBar : MonoBehaviour
{
    private IObjectPool<UIHpBar> hpBarObjectPool;

    [SerializeField]
    private Slider hpBarSlider;

    private CharactorStats targetStats;

    public void OnChangeHpBar(float persent)
    {
        hpBarSlider.value = persent;
    }

    public void SetPool(IObjectPool<UIHpBar> hpBarObjectPool)
    {
        this.hpBarObjectPool = hpBarObjectPool;
    }

    public void SetTarget(CharactorStats charactorStats)
    {
        var hpStat = charactorStats.CurrentStatTable[StatType.HP];
        hpStat.OnChangeValue += OnChangeHpBar;
        OnChangeHpBar(hpStat.PersentValue);

        charactorStats.onChangeHpEvnet.AddListener(OnChangeHpBar);
        targetStats = charactorStats;
    }

    private void OnDisable()
    {
        targetStats.CurrentStatTable[StatType.HP].OnChangeValue -= OnChangeHpBar;
    }
}
