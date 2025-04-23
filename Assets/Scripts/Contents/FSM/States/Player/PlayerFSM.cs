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
    public Inventory PlayerInventory { get; private set; }
    [field: SerializeField]
    public Transform AttackPoint {  get; private set; }
    [SerializeField]
    private LayerMask interactableTargetLayerMask;

    private Collider[] interactableTargets = new Collider[5];

    public bool UseMove { get; private set; }

    public bool IsAttack { get; private set; }

    public Vector2 MoveValue { get; private set; }

    public bool IsFindTarget { get; private set; }

    public bool IsInputAttack { get; private set; } = false;

    public bool IsPlayerInRange { get; private set; }

    public UnityEvent<GameObject> onTargetInteractEvent;
    public UnityEvent<DropItemInfo> onDropItemEvent;
    public UnityAction<int> onActAction;

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

        OnSetUseMove(true);
        OnEndAttack();

        IsFindTarget = false;
        IsPlayerInRange = false;
    }

    private void Start()
    {
        onTargetInteractEvent.AddListener(((PlayerInteractState)StateTable[PlayerStateType.Interact]).OnSetTarget);
    }

    private void Update()
    {
        StateTable[currentStateType].ExecuteUpdate();
    }

    private void FixedUpdate()
    {
        StateTable[currentStateType].ExecuteFixedUpdate();
    }

    // TODO :: // TODO :: TestPlayer -> PlayerInputHandler -> On Move And Rotate Event에 연결
    public void OnSetMoveValue(Vector2 moveValue)
    {
        this.MoveValue = moveValue;
    }

    // TODO :: TestPlayer -> PlayerInputHandler -> On Attack Event에 연결
    public void OnInputAttack(bool isInput)
    {
        IsInputAttack = isInput;
        if (!IsAttack && IsInputAttack)
        {
            IsAttack = true;
            ChangeState(PlayerStateType.Attack);
        }
    }

    // TODO :: TestPlayer -> PlayerInputHandler -> On Interact Event에 연결
    public void OnInputInteract()
    {
        if(currentStateType != PlayerStateType.Interact)
        {
            OnFindInteractableTarget();
        }
    }

    // TODO :: 지금은 TestObject 스크립트에서 사용 중
    public void OnSetIsPlayerInRange(bool value)
    {
        IsPlayerInRange = value;
    }

    public void OnSetUseMove(bool value)
    {
        UseMove = value;
    }

    public void OnDeath()
    {
        ChangeState(PlayerStateType.Death);
    }

    public void OnResurrection()
    {
        GetComponent<PlayerStats>().OnResurrection();
        ChangeState(PlayerStateType.Idle);
    }

    public void OnEndAttack()
    {
        IsAttack = false;
    }
    public void OnShowInventory()
    {
        PlayerInventory.gameObject.SetActive(true);        
    }

    public void OnDisableInventory()
    {
        PlayerInventory.gameObject.SetActive(false);
    }

    public void OnDropItem(DropItemInfo dropItemInfo)
    {
        PlayerInventory.AddItem(dropItemInfo);
        // onDropItemEvent?.Invoke(dropItemInfo);
    }

    public void OnPlayAct(int id)
    {
        onActAction?.Invoke(id);
    }

    private void OnFindInteractableTarget()
    {
        // 상호작용 가능한 대상 탐색
        int index = Physics.OverlapSphereNonAlloc(transform.position, 2f, interactableTargets, interactableTargetLayerMask);

        float closestDistance = float.MaxValue;
        GameObject closestTarget = null;

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

            float distance = Vector3.Distance(transform.position, interactableTargets[i].transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = interactableTargets[i].gameObject;
            }
        }

        if (closestTarget != null)
        {
            onTargetInteractEvent?.Invoke(closestTarget);
            ChangeState(PlayerStateType.Interact);
        }

    }

    public void Save()
    {
        SaveLoadManager.Data.playerSaveInfo.position = transform.position;
        SaveLoadManager.Data.playerSaveInfo.hp = GetComponent<PlayerStats>().CurrentStatTable[StatType.HP].Value;

        if(SaveLoadManager.Data.playerSaveInfo.survivalStatValues == null)
        {
            SaveLoadManager.Data.playerSaveInfo.survivalStatValues = new float[(int)SurvivalStatType.End];
        }
        GetComponent<PlayerStats>().Save();
        PlayerInventory.Save();
    }

    public void Load()
    {
        if (SaveLoadManager.Data != null)
        {
            transform.position = SaveLoadManager.Data.playerSaveInfo.position;
        }

        PlayerInventory.Initialize();
    }
}
