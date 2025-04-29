using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedLifeExperience : MonoBehaviour, IDestructible
{ 
    public void OnDestruction(GameObject attacker)
    {
        if(attacker.GetComponent<LifeStat>() == null)
        {
            return;
        }

        var experience = GetComponent<IExperience>();
        if(experience != null)
        {
            attacker.GetComponent<LifeStat>().OnAddExperience(this.gameObject, experience.Experience);
        }
    }
}
