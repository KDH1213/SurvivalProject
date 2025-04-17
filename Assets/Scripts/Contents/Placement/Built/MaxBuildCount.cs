using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BuildCount", menuName ="Placement/buildCount")]
public class MaxBuildCount : ScriptableObject
{
    [SerializeField]
    public SerializedDictionary<int, int> buildCounts;
}
