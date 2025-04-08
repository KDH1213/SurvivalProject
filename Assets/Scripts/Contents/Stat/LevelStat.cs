using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelStat : MonoBehaviour
{
    [SerializeField]
    protected int currentLevel = 1;
    [SerializeField]
    protected int maxLevel = 100;

    protected float currentExperience;
    protected float levelUpExperience;

    protected abstract void LevelUp();
}
