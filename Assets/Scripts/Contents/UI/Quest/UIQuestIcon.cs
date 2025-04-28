using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIQuestIcon : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent<UIQuestIcon> OnDisabledEvent { get; set; }

    private Vector2 ingamePosition;
    private RectTransform rectTransform;
    private RectTransform minimapRectTransform;

    private Transform targetTransform;

    private Vector2 mapRatio;
    private Vector2 sizeDelta;
    private Vector2 uiPosition;

    private bool isMarker = false;
    private bool isIconRender = true;
    private bool isRenderArea = false;

    private float questAreaRadius;
    private float maskRadius;

    [SerializeField]
    private RectTransform questAreaCircle;

    [SerializeField]
    private RectTransform questIcon;

    private void OnDisable()
    {
        OnDisabledEvent?.Invoke(this);
    }


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        sizeDelta = rectTransform.rect.size * 0.5f;
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
            sizeDelta = questAreaCircle.sizeDelta;
        }
        
        if(UseQuestMarkRangeInGame)
        {

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
    }
}
