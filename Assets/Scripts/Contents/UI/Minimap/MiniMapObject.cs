using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniMapObject : MonoBehaviour, IMiniMap
{
    [field: SerializeField]
    public UnityEvent<GameObject> OnActiveEvent { get; set; }

    [field: SerializeField]
    public UnityEvent<GameObject> OnDisabledEvent { get; set; }

    [field: SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    public bool IsStatic {  get; private set; }

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        var miniMapController = GameObject.FindWithTag("MiniMap").GetComponent<MiniMapController>();
        miniMapController.AddObject(this);
    }

    protected virtual void OnEnable()
    {
        OnActiveEvent?.Invoke(gameObject);
    }

    protected virtual void OnDisable()
    {
        OnDisabledEvent?.Invoke(gameObject);
    }

    protected void OnDestroy()
    {
        OnDisabledEvent.RemoveAllListeners();
    }

    private void OnApplicationQuit()
    {
        OnDisabledEvent.RemoveAllListeners();
    }
}
