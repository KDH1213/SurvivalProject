using UnityEngine;

public class MonsterDeathState : MonsterBaseState
{
    //[SerializeField]
    //private Inventory inventory;

    // public List<ItemData> items;
    private Collider ownerCollider;


    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Death;
        ownerCollider = GetComponent<Collider>();
    }

    public override void Enter()
    {
        MonsterFSM.OnEndInteractEvent?.Invoke(gameObject);

        MonsterFSM.Animator.SetTrigger(MonsterAnimationHashCode.hashDeath);
        Agent.isStopped = true;
        Agent.speed = 0f;
        Agent.destination = transform.position;
        Agent.velocity = Vector3.zero;
        // MonsterFSM.Animator.SetBool(MonsterAnimationHashCode.hashIsDeath, true);

        ownerCollider.enabled = false;
    }

    public override void ExecuteUpdate()
    {
    }

    public override void Exit()
    {

    }

    public void OnEndAnimationEvent()
    {
        gameObject.SetActive(false);
        ownerCollider.enabled = true;
    }
}
