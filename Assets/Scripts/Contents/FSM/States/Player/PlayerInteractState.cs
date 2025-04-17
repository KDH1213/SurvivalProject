using UnityEngine;
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

    private IInteractable targetInteractable;

    protected override void Awake()
    {
        base.Awake();
        stateType = PlayerStateType.Interact;
    }

    public override void Enter()
    {
        // InteractObject();
        //playerFSM.ChangeState(PlayerStateType.Idle);

        currentTime = Time.time + interactTime;
        playerFSM.Animator.SetBool(PlayerAnimationHashCode.hashMove, false);

        if (interactSilder != null)
        {
            interactSilder.gameObject.SetActive(true);
        }

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
            case InteractType.Branch:
            case InteractType.Rock:
            case InteractType.Bush:
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

    public override void ExecuteUpdate()
    {
        if (Time.time > currentTime)
        {
            InteractObject();
            playerFSM.ChangeState(PlayerStateType.Idle);
        }
        else
        {
            if (PlayerFSM.MoveValue.sqrMagnitude > 0f)
            {
                PlayerFSM.ChangeState(PlayerStateType.Move);
            }
        }
        if (interactSilder != null)
        {
            interactSilder.value = (interactTime - (currentTime - Time.time)) / interactTime;
        }
    }

    public override void Exit()
    {
        playerFSM.Animator.SetBool(PlayerAnimationHashCode.hashIsFarming, false);
        playerFSM.Animator.SetBool(PlayerAnimationHashCode.hashIsPickingUp, false);
        playerFSM.Animator.SetBool(PlayerAnimationHashCode.hashIsAxing, false);

        if (interactSilder != null)
        {
            interactSilder.gameObject.SetActive(false);
            axe.SetActive(false);
            pickaxe.SetActive(false);
        }
    }

    public void InteractObject()
    {
        if(target != null)
        {
            targetInteractable.Interact(this.gameObject);

            if (targetInteractable.InteractType <= InteractType.Box)
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

    public void OnSetTarget(GameObject target)
    {
        this.target = target;
        targetInteractable = target.GetComponent<IInteractable>();

        var targetPosition = target.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
        // 타겟 별 애니메이션 호출

    }
}
