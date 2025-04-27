using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedLifeExperience : MonoBehaviour, IDestructible
{ 
    // TODO :: 데이터 테이블 추가 시 삭제 예정

    [SerializeField]
    private float experience;

    public void OnDestruction(GameObject attacker)
    {
        if(attacker.GetComponent<LifeStat>() == null)
        {
            return;
        }
        attacker.GetComponent<LifeStat>().OnAddExperience(this.gameObject, experience);
    }
}
