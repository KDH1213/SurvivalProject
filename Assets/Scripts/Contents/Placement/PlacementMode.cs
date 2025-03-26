using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementMode : MonoBehaviour
{
    [SerializeField]
    private GameObject gridVisualization;
    [SerializeField]
    private GameObject ObjectListUi;
    [SerializeField]
    private Canvas canvas;

    private void Start()
    {
        OutPlacementMode();
    }
    public void SetPlacementMode()
    {
        // 이후 작업 모드에 들어갈 때 필요한 세팅 작성
        gridVisualization.SetActive(true);
        ShowObjectList();
    }

    public void OutPlacementMode()
    {
        // 이후 작업 모드에서 나올 때 필요한 세팅 작성
        gridVisualization.SetActive(false);
        StopShowObjectList();
    }
    
    public void ShowObjectList()
    {
        RectTransform rectTran = ObjectListUi.GetComponent<RectTransform>();
        Vector3 uiPos = Camera.main.WorldToScreenPoint(ObjectListUi.transform.position);
        ObjectListUi.transform.DOMoveY(0, 0.3f);
    }
    public void StopShowObjectList()
    {
        RectTransform rectTran = ObjectListUi.GetComponent<RectTransform>();
        Vector3 uiPos = Camera.main.ScreenToWorldPoint(new Vector3(0, -rectTran.offsetMax.y));
        
        ObjectListUi.transform.DOLocalMoveY(ObjectListUi.transform.localPosition.y - rectTran.offsetMax.y, 0.3f);
        
    }
}
