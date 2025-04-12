using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedLifeExperience : MonoBehaviour, IDestructible
{ 
    // TODO :: ������ ���̺� �߰� �� ���� ����

    [SerializeField]
    private float experience;

    // TODO :: ������ ���̺��� ����� ID
    private int id;

    public void OnDestruction(GameObject attacker)
    {
        if(attacker.GetComponent<LifeStat>() == null)
        {
            return;
        }
        attacker.GetComponent<LifeStat>().OnAddExperience(experience);
    }
}
