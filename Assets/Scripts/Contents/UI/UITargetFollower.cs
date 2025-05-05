using UnityEngine;

public class UITargetFollower : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 targetOffset;

    private RectTransform rectTransform;
    private RectTransform canvasRectTransform;
    private Vector2 resultTargetAnchor;

    private Camera mainCamera;

    public void SetTarget(Transform target, Vector3 offset)
    {
        if (rectTransform == null)
        {
            Initialize();
        }

        this.target = target;
        targetOffset = offset;
        mainCamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();

        if (target != null)
        {
            CalculateTargetAnchorPosition();
            rectTransform.anchoredPosition = resultTargetAnchor;
        }
    }

    public void SetCanvas(RectTransform rectTransform)
    {
        canvasRectTransform = rectTransform;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position + targetOffset);

            // CalculateTargetAnchorPosition(); 
            // rectTransform.anchoredPosition = resultTargetAnchor;
        }
    }

    //private void FixedUpdate()
    //{
    //    if (target != null)
    //    {
    //        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position + targetOffset);

    //        transform.position = screenPos;
    //        //CalculateTargetAnchorPosition();
    //        //rectTransform.anchoredPosition = resultTargetAnchor;
    //    }
    //}

    private void CalculateTargetAnchorPosition()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(target.position + targetOffset);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, mainCamera, out resultTargetAnchor);
    }

    private void Initialize()
    {
        rectTransform = GetComponent<RectTransform>();
        // mainCamera = Camera.main;
    }

}
