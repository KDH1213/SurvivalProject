using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedCraftingExperience : MonoBehaviour, IDestructible
{
    // TODO :: ������ ���̺� �߰� �� ���� ����
    
    [SerializeField]
    private float experience;

    // TODO :: ������ ���̺��� ����� ID
    private int id;

    public void OnDestruction(GameObject attacker)
    {
        attacker.GetComponent<CraftingStat>().OnAddExperience(experience);
    }
}
