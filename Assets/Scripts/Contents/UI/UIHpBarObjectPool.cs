using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UIHpBarObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject hpBarPrefab;

    [SerializeField]
    private Transform createPoint;

    [SerializeField]
    private RectTransform hpBarCanvasRectTransform;
    public Dictionary<int, IObjectPool<UIHpBar>> uIHpBarPoolTable { get; private set; } = new Dictionary<int, IObjectPool<UIHpBar>>();

    public IObjectPool<UIHpBar> HpBarObjectPool { get; private set; }

    private void Awake()
    {
        HpBarObjectPool = new ObjectPool<UIHpBar>(OnCreateHpBar, OnGetHpBar, OnReleaseHpBar, OnDestroyHpBar, true, 100);
        ObjectPoolManager.Instance.AddObjectPool(ObjectPoolType.HpBar, this);
    }

    private UIHpBar OnCreateHpBar()
    {
        Instantiate(hpBarPrefab, createPoint).TryGetComponent(out UIHpBar uIHpBar);
        uIHpBar.SetPool(HpBarObjectPool);
        uIHpBar.gameObject.SetActive(true);
        uIHpBar.gameObject.GetComponent<UITargetFollower>().SetCanvas(hpBarCanvasRectTransform);

        return uIHpBar;
    }

    private void OnGetHpBar(UIHpBar uIHpBar)
    {
        uIHpBar.gameObject.SetActive(true);
    }

    private void OnReleaseHpBar(UIHpBar uIHpBar)
    {
        uIHpBar.gameObject.SetActive(false);
    }

    private void OnDestroyHpBar(UIHpBar uIHpBar)
    {
        Destroy(uIHpBar.gameObject);
    }

    public UIHpBar GetHpBar()
    {
        return HpBarObjectPool.Get();
    }
}
