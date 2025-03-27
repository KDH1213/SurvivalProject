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
        // ���� �۾� ��忡 �� �� �ʿ��� ���� �ۼ�
        gridVisualization.SetActive(true);
        ShowObjectList();
    }

    public void OutPlacementMode()
    {
        // ���� �۾� ��忡�� ���� �� �ʿ��� ���� �ۼ�
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
