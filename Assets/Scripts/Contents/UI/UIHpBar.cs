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
        charactorStats.onChangeHpEvnet.AddListener(OnChangeHpBar);
        targetStats = charactorStats;
    }

    private void OnDisable()
    {
        targetStats.onChangeHpEvnet.RemoveListener(OnChangeHpBar);
    }
}
