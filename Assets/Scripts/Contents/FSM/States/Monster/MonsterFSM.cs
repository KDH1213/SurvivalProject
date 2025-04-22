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

    public MonsterData MonsterData { get; private set; } = null;

    [HideInInspector]
    public GameObject Target { get; set; }

    [HideInInspector]
    public Transform TargetTransform { get; set; }

    [field: SerializeField]
    public Transform AttackPoint { get; private set; }

    public Vector3 FirstPosition { get; private set; }

    public bool CanAttack { get; private set; }

    public bool IsAttack { get; private set; }

    public float TargetDistance { get; set; }


    public bool IsDead { get; private set; }
    public bool CanRouting { get; private set; }

    public bool IsInteractable => IsDead;
    public IObjectPool<MonsterFSM> MonsterPool { get; private set; } = null;
    public UnityEvent<GameObject> OnEndInteractEvent => onEndInteractEvent;
    public UnityEvent<GameObject> onEndInteractEvent;

    public InteractType InteractType => InteractType.Monster;

    public Vector3 RespawnPosition { get { return FirstPosition; } private set { FirstPosition = value; } }

    public float RespawnTime { get; private set; } = 10f;

    public float RemainingTime { get; private set; }

    public bool IsRespawn => IsDead;

    public float InteractTime => 0f;

    protected override void Awake()
    {
        if(MonsterData == null)
        {
            MonsterData = DataTableManager.MonsterTable.Get(ID);
        }

        CanAttack = true;
        IsAttack = false;
        IsDead = false;
        CanRouting = false;
        FirstPosition = gameObject.transform.position;
    }

    private void OnEnable()
    {
        if (IsDead)
        {
            IsDead = false;
            transform.position = FirstPosition;
            ChangeState(MonsterStateType.Idle);
        }
    }

    private void Update()
    {
        StateTable[currentStateType].ExecuteUpdate();
    }

    private void FixedUpdate()
    {
        StateTable[currentStateType].ExecuteFixedUpdate();
    }
    public void SetCanAttack(bool value)
    {
        CanAttack = value;
    }

    public void SetIsAttack(bool value)
    {
        IsAttack = value;
    }

    // TODO :: TestMonster -> DestructedEvent �̺�Ʈ�� ����
    public void OnDeath()
    {
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
