using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemDropView : MonoBehaviour
{
    [SerializeField]
    private UIItemDropInfo itemDropInfoPrefab;

    [SerializeField]
    private Transform createPoint;

    [SerializeField]
    private Queue<UIItemDropInfo> uIItemDropInfoQueue = new Queue<UIItemDropInfo>();

    [SerializeField]
    private List<UIItemDropInfo> disableUIItemDropInfoList = new List<UIItemDropInfo>();

    private float lastDisableTime;
    [SerializeField]
    private float lifeTime;

    [SerializeField]
    private float extraTime = 0.1f;

    private void Awake()
    {
        GameObject.FindWithTag(Tags.Player).GetComponent<PlayerFSM>().onDropItemEvent.AddListener(OnItemDrop);
    }

    public void OnItemDrop(DropItemInfo dropItemInfo)
    {
        int count = disableUIItemDropInfoList.Count;
        UIItemDropInfo uIItemDropInfo = null;
        if (count != 0)
        {
            uIItemDropInfo = disableUIItemDropInfoList[count - 1];
            disableUIItemDropInfoList.Remove(uIItemDropInfo);
        }
        else
        {
            uIItemDropInfo = Instantiate(itemDropInfoPrefab, createPoint);
            uIItemDropInfo.diableAction += OnEndItemInfo;
        }

        if(uIItemDropInfoQueue.Count == 0)
        {
            lastDisableTime = Time.time + lifeTime;
        }
        else
        {
            float time = Time.time + lifeTime - lastDisableTime;
            if(time < extraTime)
            {
                lastDisableTime += extraTime;
            }
        }

        uIItemDropInfoQueue.Enqueue(uIItemDropInfo);
        uIItemDropInfo.SetDropItemInfo(dropItemInfo.itemData.ItemImage, dropItemInfo.amount, lastDisableTime);
    }

    public void OnEndItemInfo()
    {
        disableUIItemDropInfoList.Add(uIItemDropInfoQueue.Dequeue());
    }
}
