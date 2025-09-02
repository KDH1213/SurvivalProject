using UnityEngine;

[ExecuteInEditMode] // 에디터 모드에서도 실행
public class CameraRenderToTexture : MonoBehaviour
{
    private Camera targetCamera;
    public RenderTexture renderTexture;

    void OnEnable()
    {
        targetCamera = gameObject.GetComponent<Camera>();

        if (targetCamera != null && renderTexture != null)
        {
            targetCamera.targetTexture = renderTexture;
        }
    }

    void OnDisable()
    {
        if (targetCamera != null)
        {
            targetCamera.targetTexture = null;
        }
    }
}