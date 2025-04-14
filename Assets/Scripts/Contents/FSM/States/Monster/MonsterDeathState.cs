public class MonsterDeathState : MonsterBaseState
{
    //[SerializeField]
    //private Inventory inventory;

    // public List<ItemData> items;

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Death;
    }

    public override void Enter()
    {
        MonsterFSM.OnEndInteractEvent?.Invoke(gameObject);

        MonsterFSM.Animator.SetTrigger(MonsterAnimationHashCode.hashDeath);
        MonsterFSM.Animator.SetBool(MonsterAnimationHashCode.hashIsDeath, true);
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
    }
}
