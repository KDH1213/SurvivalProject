using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gather : MonoBehaviour, IInteractable, IRespawn
{
    [SerializeField]
    private InteractType interactType;

    public bool IsInteractable => gameObject.activeSelf;

    public InteractType InteractType => interactType;

    public Vector3 RespawnPosition { get; set; }

    public float RespawnTime { get; private set; } = 10f;

    public float RemainingTime { get; private set; }

    public bool IsRespawn { get; private set; } = true;

    public UnityEvent<GameObject> OnEndInteractEvent => onEndInteractEvent;
    public UnityEvent<GameObject> onEndInteractEvent;

    private void Awake()
    {
        RespawnPosition = transform.position;
    }

    private void OnEnable()
    {
        transform.position = RespawnPosition;
        IsRespawn = true;
        RemainingTime = 0f;
    }

    public void Interact(GameObject interactor)
    {
        IsRespawn = false;
        gameObject.SetActive(false);
        OnEndInteractEvent?.Invoke(gameObject);
    }

    public void SetRemainTime(float remainTime)
    {
        RemainingTime = remainTime;
        IsRespawn = false;
    }
}
