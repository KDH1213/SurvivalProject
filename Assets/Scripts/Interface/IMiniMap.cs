using UnityEngine;
using UnityEngine.Events;

public interface IMiniMap
{
    UnityEvent<GameObject> OnActiveEvent { get; }
    UnityEvent<GameObject> OnDisabledEvent { get; }
    Sprite Icon { get; }
    bool IsStatic { get; }
    bool IsMarker { get; }
}