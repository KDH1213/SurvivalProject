using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIQuestIcon : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent<UIQuestIcon> OnDisabledEvent { get; set; }

    [SerializeField]
    private RectTransform questAreaCircle;

    [SerializeField]
    private RectTransform questIcon;

    [SerializeField]
    private GameObject ingameQuestRangePrefab;

    private Vector2 ingamePosition;
    private RectTransform rectTransform;
    private RectTransform minimapRectTransform;

    private Transform targetTransform;
    private GameObject ingameQuestRange;

    private Vector2 mapRatio;
    private Vector2 sizeDelta;
    private Vector2 uiPosition;

    private bool isMarker = false;
    private bool isIconRender = true;
    private bool isRenderArea = false;

    private float questAreaRadius;
    private float maskRadius;


    private void OnDisable()
    {
        OnDisabledEvent?.Invoke(this);
    }


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        sizeDelta = questIcon.sizeDelta;
    }

    private void Update()
    {
        var targetPosition = targetTransform.position.ConvertVector2() * mapRatio;
        var direction = targetPosition - uiPosition;
        var distance = direction.magnitude;

        if (distance + questAreaRadius > maskRadius)
        {
            if(isRenderArea && isIconRender)
            {
                questIcon.gameObject.SetActive(true);
                questAreaCircle.gameObject.SetActive(false);
            }

            direction.Normalize();
            rectTransform.anchoredPosition = targetPosition - (direction * maskRadius) + (sizeDelta * direction);
        }
        else
        {
            if (isRenderArea)
            {
                questIcon.gameObject.SetActive(false);
                questAreaCircle.gameObject.SetActive(true);
            }

            rectTransform.anchoredPosition = uiPosition;
        }
    }

    public void SetQuestInfo(Vector2 position, Vector2 ratio, bool UseQuestMarkRangeInGame = false, float questAreaRadius = 0f)
    {
        ingamePosition = position;

        rectTransform.anchoredPosition = ingamePosition * ratio;
        uiPosition = rectTransform.anchoredPosition;
        mapRatio = ratio;

        this.questAreaRadius = questAreaRadius * 2f;

        if(questAreaRadius > 0f)
        {
            isRenderArea = true;
            questAreaCircle.sizeDelta = new Vector2(this.questAreaRadius, this.questAreaRadius) * mapRatio;
        }
        
        if(UseQuestMarkRangeInGame)
        {
            if(ingameQuestRange == null)
            {
                ingameQuestRange = Instantiate(ingameQuestRangePrefab);
            }
            else
            {
                ingameQuestRange.SetActive(true);
            }

            ingameQuestRange.transform.position = new Vector3(position.x, 0.1f, position.y);
            ingameQuestRange.transform.localScale = new Vector3(questAreaRadius, questAreaRadius, questAreaRadius);
        }
    }

    public void SetMinimapInfo(Transform target, RectTransform minimapRectTransform, Rect maskRect)
    {
        targetTransform = target;
        this.minimapRectTransform = minimapRectTransform;
        maskRadius = maskRect.width * 0.5f;
    }

    public void SetActiveMarker(bool isMarker)
    {
        this.isMarker = isMarker;

        if (isMarker)
        {
            enabled = true;
        }
    }

    public void OnQuestClear()
    {
        gameObject.SetActive(false);

        if(ingameQuestRange != null)
        {
            ingameQuestRange.SetActive(false);
        }
    }
}
