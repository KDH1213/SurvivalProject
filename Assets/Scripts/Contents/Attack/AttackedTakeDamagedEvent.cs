using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackedTakeDamagedEvent : MonoBehaviour, IAttackable
{
    private CharactorStats charactorStats;
    public UnityEvent<GameObject> onAttackerEvent;

    private void Awake()
    {
        charactorStats = GetComponent<CharactorStats>();
    }

    public void OnAttack(GameObject attacker, DamageInfo attack)
    {
        onAttackerEvent?.Invoke(attacker);
    }
}
