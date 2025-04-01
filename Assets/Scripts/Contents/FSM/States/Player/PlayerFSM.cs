using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFSM : FSMController<PlayerStateType>, ISaveLoadData
{
    [field: SerializeField]
    public Animator Animator { get; private set; }
    [field: SerializeField]
    public CharacterController CharacterController { get; private set; }
    [field: SerializeField]
    public Weapon Weapon { get; private set; }
    [field: SerializeField]
    public Data PlayerData { get; private set; }
    [field: SerializeField]
    public GameObject PlayerInventory { get; private set; }

    [SerializeField]
    private LayerMask interactableTargetLayerMask;

    private Collider[] interactableTargets = new Collider[5];
    public GameObject InteractableTarget { get; set; }

    public HashSet<GameObject> AttackTargets { get; set; } = new HashSet<GameObject>();

    public bool UseMove { get; private set; }

    public bool CanAttack { get; private set; }

    public bool IsAttack { get; private set; }

    public Vector2 MoveValue { get; private set; }

    public bool IsFindTarget { get; private set; }

    [HideInInspector]
    public float attackRange;

    public bool IsPlayerInRange { get; private set; }

    public UnityEvent<GameObject> onTargetInteractEvent;

    protected override void Awake()
    {
        if (SaveLoadManager.Data != null)
        {
            Load();
        }

        var stageManager = GameObject.FindGameObjectWithTag("StageManager");
        if (stageManager != null)
        {
            stageManager.GetComponent<StageManager>().onSaveEvent += Save;
        }

        SetCanAttack(true);
        OnSetUseMove(true);
        OnEndAttack();

        IsFindTarget = false;
        IsPlayerInRange = false;
        attackRange = 2f;
    }

    private void Start()
    {
        onTargetInteractEvent.AddListener(((PlayerInteractState)StateTable[PlayerStateType.Interact]).OnSetTarget);
    }

    private void Update()
    {
        StateTable[currentStateType].ExecuteUpdate();
    }

    // TODO :: // TODO :: TestPlayer -> PlayerInputHandler -> On Move And Rotate Event에 연결
    public void OnSetMoveValue(Vector2 moveValue)
    {
        this.MoveValue = moveValue;
    }

    // TODO :: TestPlayer -> PlayerInputHandler -> On Attack Event에 연결
    public void OnInputAttack()
    {
        if (CanAttack)
        {
            ChangeState(PlayerStateType.Attack);
        }
    }

    // TODO :: TestPlayer -> PlayerInputHandler -> On Interact Event에 연결
    public void OnSetInteract()
    {
        FindInteractableTarget();

        // TODO :: 임시 코드
        if (InteractableTarget != null)
        {
            onTargetInteractEvent?.Invoke(InteractableTarget);

            ChangeState(PlayerStateType.Interact);
        }

    }

    // TODO :: 지금은 TestObject 스크립트에서 사용 중
    public void OnSetIsPlayerInRange(bool value)
    {
        IsPlayerInRange = value;
    }

    public void SetCanAttack(bool value)
    {
        CanAttack = value;
    }

    public void OnSetUseMove(bool value)
    {
        UseMove = value;
    }

    public void OnDeath()
    {
        ChangeState(PlayerStateType.Idle);
    }

    public void OnEndAttack()
    {
        IsAttack = false;
    }

    // TODO :: 임시 / CancleButton의 On Click 이벤트에 연결
    public void OnChangeIdleState()
    {
        ChangeState(PlayerStateType.Idle);
    }
    
    public void OnShowInventory()
    {
        PlayerInventory.gameObject.SetActive(true);
    }

    public void OnDisableInventory()
    {
        PlayerInventory.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }


    private void FindInteractableTarget()
    {
        if (InteractableTarget != null)
        {
            IInteractable currentTarget = InteractableTarget.GetComponent<IInteractable>();
            if (currentTarget == null || !currentTarget.IsInteractable)
            {
                InteractableTarget = null;
            }
            else
            {
                return;
            }
        }

        // 상호작용 가능한 대상 탐색
        int index = Physics.OverlapSphereNonAlloc(transform.position, attackRange, interactableTargets, interactableTargetLayerMask);

        float closestDistance = float.MaxValue;
        GameObject closestTarget = null;
        Vector3 forward = transform.forward; // 플레이어가 바라보는 방향

        for (int i = 0; i < index; ++i)
        {
            if (interactableTargets[i] == null)
            {
                break;
            }

            IInteractable target = interactableTargets[i].GetComponent<IInteractable>();
            if (target == null || !target.IsInteractable)
            {
                continue;
            }

            Vector3 directionToTarget = (interactableTargets[i].transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, interactableTargets[i].transform.position);

            // 전방 90도(±45도) 범위 안에 있는지 확인
            if (Vector3.Dot(forward, directionToTarget) > Mathf.Cos(Mathf.Deg2Rad * 45))
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = interactableTargets[i].gameObject;
                }
            }
        }

        if (closestTarget != null)
        {
            InteractableTarget = closestTarget;
            IInteractable target = InteractableTarget.GetComponent<IInteractable>();
            target.Interact(gameObject);
        }
    }

    public void Save()
    {
    }

    public void Load()
    {
    }
}
