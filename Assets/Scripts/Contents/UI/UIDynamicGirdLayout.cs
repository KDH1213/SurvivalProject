using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDynamicGirdLayout : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;
    [SerializeField]
    private RectTransform parentRectTransform;

    private void OnEnable()
    {
        // gridLayoutGroup.constraintCount
    }

}
