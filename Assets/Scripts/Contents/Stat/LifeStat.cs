using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStat : LevelStat, ISaveLoadData
{
    // private SerializedDictionary<, int>

    public void OnAddExperience(float experience)
    {
        currentExperience += experience;
    }
    public void Load()
    {
        throw new System.NotImplementedException();
    }

    public void Save()
    {
        throw new System.NotImplementedException();
    }

    protected override void LevelUp()
    {
        throw new System.NotImplementedException();
    }
}
