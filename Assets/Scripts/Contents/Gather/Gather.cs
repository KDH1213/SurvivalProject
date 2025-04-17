using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gather : MonoBehaviour, IInteractable, IRespawn
{
    [SerializeField]
    private GameObject diskObject;

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

    [ContextMenu("CreateDisk")]
    public void OnSetDisk()
    {
        //if (transform.childCount > 0)
        //{
        //    diskObject = transform.GetChild(0).gameObject;
        //}
        //else
        //{
        //    diskObject = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)(Resources.Load("Prefabs/Disk", typeof(GameObject))), transform);
        //}
        diskObject.SetActive(true);
    }

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

        interactor.GetComponent<IAct>()?.OnPlayAct((int)ActType.Interation);
    }

    public void SetRemainTime(float remainTime)
    {
        RemainingTime = remainTime;
        IsRespawn = true;
    }

    public void LoadData(GatherSaveInfo gatherSaveInfo)
    {
        IsRespawn = gatherSaveInfo.isRespawn;
        RemainingTime = gatherSaveInfo.remainingTime;
        transform.position = gatherSaveInfo.position;
        RespawnPosition = gatherSaveInfo.respawnPosition;

        if(IsRespawn)
        {
            gameObject.SetActive(false);
        }
    }
}
