using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerInteractState : PlayerBaseState
{
    [SerializeField]
    private Slider interactSilder;

    [SerializeField]
    private GameObject axe;
    [SerializeField]
    private GameObject pickaxe;

    private GameObject target;
    private float interactTime = 2f;
    private float currentTime = 0f;

    private NavMeshAgent agent;

    private float interactRange = 1f;
    private bool isIntercting = false;

    private IInteractable targetInteractable;

    private Vector3 closestPoint;

    private List<ItemSlotInfo> gatherItemSlotInfoList;
    private EquipmentSocket weaponEquipmentSocket;

    private SFXPlayer soundPlayer;
    protected override void Awake()
    {
        base.Awake();
        stateType = PlayerStateType.Interact;
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        if (targetInteractable.InteractType == InteractType.Tree)
        {
            gatherItemSlotInfoList = playerFSM.PlayerInventory.GatherItemSlotInfoList[0];
          
            var count = gatherItemSlotInfoList.Count;
            var weaponData = weaponEquipmentSocket.ItemInfo.itemData != null ? DataTableManager.WeaponTable.Get(weaponEquipmentSocket.ItemInfo.itemData.ID) : null;
        
            if (count == 0 && (weaponData == null || weaponData.GatherType != 1))
            {
                playerFSM.ChangeState(PlayerStateType.Idle);
                ToastMsg.Instance.ShowMessage("도끼류 장비가 착용되어 있지 않습니다!", Color.red);
                return;
            }
        }
        else if (targetInteractable.InteractType == InteractType.Stone)
        {
            gatherItemSlotInfoList = playerFSM.PlayerInventory.GatherItemSlotInfoList[1];
            var weaponData = weaponEquipmentSocket.ItemInfo.itemData != null ? DataTableManager.WeaponTable.Get(weaponEquipmentSocket.ItemInfo.itemData.ID) : null;
            var count = gatherItemSlotInfoList.Count;
        
            if (count == 0 && (weaponData == null || weaponData.GatherType != 1))
            {
                playerFSM.ChangeState(PlayerStateType.Idle);
                ToastMsg.Instance.ShowMessage("곡괭이류 장비가 착용되어 있지 않습니다!", Color.red);
                return;
            }
        }

        Collider collider = target.GetComponentInChildren<Collider>();
        closestPoint = collider.ClosestPoint(transform.position);
        float distance = (closestPoint - transform.position).ConvertVector2().magnitude;

        if (distance > interactRange)
        {
            PlayerFSM.Animator.SetFloat(PlayerAnimationHashCode.hashSpeed, PlayerStats.Speed);
            agent.speed = PlayerStats.Speed;
            agent.isStopped = false;
            agent.SetDestination(closestPoint);
        }
        else
        {
            PlayInteract();
        }
    }

    public override void ExecuteUpdate()
    {
        if (PlayerFSM.MoveValue.sqrMagnitude > 0f)
        {
            PlayerFSM.ChangeState(PlayerStateType.Move);
            return;
        }

        if (!isIntercting)
        {
            var targetPosition = target.transform.position;
            float distance = (closestPoint - transform.position).ConvertVector2().magnitude;
            PlayerFSM.Animator.SetFloat(PlayerAnimationHashCode.hashSpeed, PlayerStats.Speed);

            if (distance < interactRange)
            {
                PlayInteract();
            }
        }
        else
        {
            if (Time.time > currentTime)
            {
                InteractObject();
                playerFSM.ChangeState(PlayerStateType.Idle);
            }

            if (interactSilder != null)
            {
                interactSilder.value = (interactTime - (currentTime - Time.time)) / interactTime;
            }
        }       
    }

    public override void Exit()
    {
        isIntercting = false; 
        agent.isStopped = true;
        agent.destination = transform.position;
        agent.velocity = Vector3.zero;

        playerFSM.Animator.SetBool(PlayerAnimationHashCode.hashIsFarming, false);
        playerFSM.Animator.SetBool(PlayerAnimationHashCode.hashIsPickingUp, false);
        playerFSM.Animator.SetBool(PlayerAnimationHashCode.hashIsAxing, false);

        PlayerFSM.onEnableWeaponEvent?.Invoke();

        if (interactSilder != null)
        {
            interactSilder.gameObject.SetActive(false);
            axe.SetActive(false);
            pickaxe.SetActive(false);
        }

        if(soundPlayer != null)
        {
            soundPlayer.StopSFX();
            soundPlayer = null;
        }
    }

    private void PlayInteract()
    {
        agent.isStopped = true;
        agent.destination = transform.position;
        agent.velocity = Vector3.zero;

        PlayerFSM.Animator.SetFloat(PlayerAnimationHashCode.hashSpeed, 0f);
        isIntercting = true;

        currentTime = Time.time + interactTime;

        if (interactSilder != null)
        {
            interactSilder.gameObject.SetActive(true);
        }

        var targetPosition = target.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);

        PlayerFSM.onDisableWeaponEvent?.Invoke();

        switch (targetInteractable.InteractType)
        {
            case InteractType.Tree:
                playerFSM.Animator.SetBool(PlayerAnimationHashCode.hashIsAxing, true);
                axe.gameObject.SetActive(true);
                break;
            case InteractType.Stone:
                playerFSM.Animator.SetBool(PlayerAnimationHashCode.hashIsFarming, true);
                pickaxe.gameObject.SetActive(true);
                break;
            case InteractType.Rock:
            case InteractType.Branch:
            case InteractType.Bush:
            case InteractType.Relics:
                playerFSM.Animator.SetBool(PlayerAnimationHashCode.hashIsPickingUp, true);
                break;
            case InteractType.Monster:
                break;
            case InteractType.Box:
            case InteractType.Placement:
                InteractObject();
                playerFSM.ChangeState(PlayerStateType.Idle);
                break;
            case InteractType.End:
                playerFSM.ChangeState(PlayerStateType.Idle);
                break;
            default:
                break;
        }
    }

    public void InteractObject()
    {
        if(target != null)
        {
            if(targetInteractable.InteractType == InteractType.Stone
                || targetInteractable.InteractType == InteractType.Tree)
            {
                var weaponData = weaponEquipmentSocket.ItemInfo.itemData != null ? DataTableManager.WeaponTable.Get(weaponEquipmentSocket.ItemInfo.itemData.ID) : null;

                if(weaponData != null &&
                    (targetInteractable.InteractType == InteractType.Stone && weaponData.GatherType == 2) 
                   || (targetInteractable.InteractType == InteractType.Tree && weaponData.GatherType == 1))
                {
                    weaponEquipmentSocket.OnUseDurability();
                }
                else
                {
                    if (gatherItemSlotInfoList.Count == 0)
                    {
                        ToastMsg.Instance.ShowMessage("필요한 장비가 없습니다!", Color.red);
                        return;
                    }

                    var itemSlotInfo = gatherItemSlotInfoList[0];
                    int count = gatherItemSlotInfoList.Count;
                    for (int i = 1; i < count; ++i)
                    {
                        if (itemSlotInfo.index > gatherItemSlotInfoList[i].index)
                        {
                            itemSlotInfo = gatherItemSlotInfoList[i];
                        }
                    }

                    itemSlotInfo.OnUseDurability();
                }           
            }

            targetInteractable.Interact(this.gameObject);

            if (targetInteractable.InteractType <= InteractType.Box || targetInteractable.InteractType == InteractType.Relics)
            {
                var IDestructible = target.GetComponent<IDestructible>();
                if (IDestructible != null)
                {
                    IDestructible.OnDestruction(this.gameObject);
                }
            }

            target = null;                    
        }

    }

    public void OnSetWeaponEquipmentSocket(EquipmentSocket weaponEquipmentSocket)
    {
        this.weaponEquipmentSocket = weaponEquipmentSocket;
    }

    public void OnSetTarget(GameObject target)
    {
        this.target = target;
        targetInteractable = target.GetComponent<IInteractable>();

        interactTime = targetInteractable.InteractTime;
        var targetPosition = target.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }

    public void OnPlayGatherSound()
    {
        if(targetInteractable.InteractType == InteractType.Tree)
        {
            soundPlayer = SoundManager.Instance.OnSFXPlay(transform, (int)SoundType.GatherTree);
        }
        else
        {
            soundPlayer = SoundManager.Instance.OnSFXPlay(transform, (int)SoundType.GatherStone);
        }
    }
}
