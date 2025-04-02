using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedCraftingExperience : MonoBehaviour, IDestructible
{
    // TODO :: 데이터 테이블 추가 시 삭제 예정
    
    [SerializeField]
    private float experience;

    // TODO :: 데이터 테이블에서 사용할 ID
    private int id;

    public void OnDestruction(GameObject attacker)
    {
        attacker.GetComponent<CraftingStat>().OnAddExperience(experience);
    }
}
