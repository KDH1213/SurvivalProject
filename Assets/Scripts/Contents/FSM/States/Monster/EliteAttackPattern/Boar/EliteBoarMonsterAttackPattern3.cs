using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EliteBoarMonsterAttackPattern3 : MonsterAttackPattern
{
    protected MonsterFSM monsterFSM;
    protected MonsterStats MonsterStats { get; private set; }
    public MonsterFSM MonsterFSM { get { return monsterFSM; } }

    [SerializeField]
    private Transform rushAttackPoint;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float rushDistance;

    [SerializeField]
    private float rushSpeed;

    [SerializeField]
    private float maxRushTime;

    private float endRushTime;
    private Vector3 rushDirection;
    private Vector3 startPosition;

    private Collider[] attackTargets = new Collider[1];

    protected virtual void Awake()
    {
        monsterFSM = GetComponent<MonsterFSM>();
        MonsterStats = GetComponent<MonsterStats>();
    }

    public override void Enter()
    {
        var targetPosition = monsterFSM.Target.transform.position;
        rushDirection = targetPosition - transform.position;
        rushDirection.y = 0f;
        rushDirection.Normalize();

        endRushTime = Time.time + maxRushTime;
        startPosition = transform.position;
        startPosition.y = 0f;

        MonsterFSM.Animator.SetBool(MonsterAnimationHashCode.hashIsRush, true);
    }

    public override void ExecuteUpdate()
    {
        transform.position += rushDirection * (rushSpeed * Time.deltaTime);

        int index = Physics.OverlapBoxNonAlloc(rushAttackPoint.position, rushAttackPoint.localScale, attackTargets, rushAttackPoint.rotation, layerMask);
        
        if(index == 0)
        {
            if(endRushTime < Time.deltaTime
            || (transform.position.ConvertVector2() - startPosition.ConvertVector2()).sqrMagnitude > rushDistance * rushDistance)
            {
                monsterFSM.ChangeState(MonsterStateType.Chase);
            }
        }
        else
        {
            MonsterFSM.Weapon.Execute(gameObject, attackTargets[0].gameObject);
            monsterFSM.ChangeState(MonsterStateType.Chase);
        }
    }

    public override void ExecuteFixedUpdate()
    {
    }


    public override void Exit()
    {
        MonsterFSM.Animator.SetBool(MonsterAnimationHashCode.hashIsRush, false);
    }
}