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

    protected override void Awake()
    {
        base.Awake();
        stateType = PlayerStateType.Interact;
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
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

        interactTime = targetInteractable.InteractTime;
        var targetPosition = target.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }
}
