using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretAttackVerdict : MonoBehaviour
{
    private readonly string triggerName = "AttackTrigger";
    private TurretAttack attack;
    private TurretType type;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float timer;
    private float attackTerm;
    private TurretStructure parent;

    private PriorityQueue<GameObject, float> attackQueue = new PriorityQueue<GameObject, float>();

    private void Update()
    {
        if(parent == null)
        {
            return;
        }
        if(parent.IsPlaced && type == TurretType.Timing && timer <= Time.time)
        {
            animator.SetTrigger(triggerName);
            timer = Time.time + attackTerm;
        }
    }

    public void SetInfo(TurretAttack attack, float attackTerm, TurretType type, TurretStructure sturcture)
    {
        parent = sturcture;
        this.attack = attack;
        this.attackTerm = attackTerm;
        this.type = type;
        timer = Time.time + attackTerm;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(type == TurretType.Timing)
        {
            AttackTiming(other.gameObject);
        }
        
    }

    private void AttackTiming(GameObject enemy)
    {
        if (enemy.tag == "Monster")
        {
            attack.Execute(transform.parent.gameObject, enemy);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        /*var parent = transform.parent.gameObject;
        if (other.tag == "Monster")
        {
            attack.Execute(parent, other.gameObject);
            attackQueue.Enqueue(other.gameObject, Time.time);
        }
        if(attackQueue.Count > 0 )
        {
            float attackTerm = parent.GetComponent<StructureStats>().CurrentStatTable[StatType.AttackSpeed].Value;
            attackQueue.TryPeek(out GameObject result, out float time);
            if(time + attackTerm > Time.time)
            {
                attackQueue.Dequeue();
            }
        }*/
    }

}
