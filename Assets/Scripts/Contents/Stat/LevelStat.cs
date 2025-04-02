using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class LevelStat : MonoBehaviour
{
    protected int currentLevel;
    protected int maxLevel;
    protected float currentExperience;
    protected float nextExperience;

    protected abstract void LevelUp();
}
