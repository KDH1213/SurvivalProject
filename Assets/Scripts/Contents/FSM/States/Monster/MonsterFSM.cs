using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Pool;

public class MonsterFSM : FSMController<MonsterStateType>, IInteractable, IRespawn
{
    [field: SerializeField]
    public int ID { get; private set; } = 0;
    [field: SerializeField]
    public Animator Animator { get; private set; }
    [field: SerializeField]
    public NavMeshAgent Agent { get; private set; } // 정리 필요
    [field: SerializeField]
    public Weapon Weapon { get; private set; }

    [field: SerializeField]
    public Data MonsterData { get; private set; }

    [HideInInspector]
    public GameObject Target { get; set; }

    [HideInInspector]
    public Transform TargetTransform { get; set; }

    [field: SerializeField]
    public Transform AttackPoint { get; private set; }

    [HideInInspector]
    public Vector3 firstPosition;

    public bool CanAttack { get; private set; }

    public bool IsChase { get; private set; }

    public bool IsAttack { get; private set; }

    public float TargetDistance { get; set; }

    public bool IsPlayerInRange { get; private set; }

    public bool IsDead { get; private set; }
    public bool CanRouting { get; private set; }

    public bool IsInteractable => IsDead;
    public IObjectPool<MonsterFSM> MonsterPool { get; private set; } = null;
    public UnityEvent<GameObject> OnEndInteractEvent => onEndInteractEvent;
    public UnityEvent<GameObject> onEndInteractEvent;

    public InteractType InteractType => InteractType.Monster;

    public Vector3 RespawnPosition { get { return firstPosition; } private set { firstPosition = value; } }

    public float RespawnTime { get; private set; } = 10f;

    public float RemainingTime { get; private set; }

    public bool IsRespawn => IsDead;

    protected override void Awake()
    {
        CanAttack = true;
        IsChase = false;
        IsPlayerInRange = false;
        IsAttack = false;
        IsDead = false;
        CanRouting = false;
        firstPosition = gameObject.transform.position;
    }

    private void OnEnable()
    {
        if (IsDead)
        {
            IsDead = false;
            transform.position = firstPosition;
            ChangeState(MonsterStateType.Idle);
        }
    }

    private void Update()
    {
        StateTable[currentStateType].ExecuteUpdate();
    }

    public void SetCanAttack(bool value)
    {
        CanAttack = value;
    }

    public void SetIsChase(bool value)
    {
        IsChase = value;
    }

    public void SetIsPlayerRange(bool value)
    {
        IsPlayerInRange = value;
    }

    public void SetIsAttack(bool value)
    {
        IsAttack = value;
    }

    // TODO :: TestMonster -> DestructedEvent �̺�Ʈ�� ����
    public void OnDeath()
    {
        Debug.Log("Monster: Die!!");
        IsDead = true;
        CanRouting = true;
        RemainingTime = RespawnTime;
        ChangeState(MonsterStateType.Death);
    }

    public void Interact(GameObject interactor)
    {
        //gameObject.SetActive(false);
        //OnEndInteractEvent?.Invoke(gameObject);
    }

    public void SetRemainTime(float remainTime)
    {
        RemainingTime = remainTime;
    }

    public void LoadData(MonsterSaveInfo monsterSaveInfo)
    {
        IsDead = monsterSaveInfo.isRespawn;
        RemainingTime = monsterSaveInfo.remainingTime;
        transform.position = monsterSaveInfo.position;
        RespawnPosition = monsterSaveInfo.respawnPosition;

        if (IsRespawn)
        {
            gameObject.SetActive(false);
        }
        else
        {
            GetComponent<MonsterStats>().LoadStats(monsterSaveInfo.hp);
        }
    }

    public void SetPool(IObjectPool<MonsterFSM> objectPool)
    {
        MonsterPool = objectPool;
    }

    public void Release()
    {
        if(MonsterPool != null)
        {
            MonsterPool.Release(this);
        }
    }
}
