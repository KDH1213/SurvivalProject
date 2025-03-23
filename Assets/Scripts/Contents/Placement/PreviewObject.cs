using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject previewObject;

    public bool IsPreview { get; private set; } 

    [SerializeField]
    private Material previewMaterialsPrefeb;
    private Material previewMaterialsInstance;

    private void Start()
    {
        previewMaterialsInstance = new Material(previewMaterialsPrefeb);
    }

    public void StartShowingPlacementPreview(GameObject prefeb)
    {
        previewObject = Instantiate(prefeb);
        PreparePreview(previewObject);
        IsPreview = true;
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialsInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        Destroy(previewObject);
        IsPreview = false;
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        ApplyFeedback(validity);
        MovePreview(position);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        c.a = 0.5f;
        previewMaterialsInstance.color = c;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }
}
