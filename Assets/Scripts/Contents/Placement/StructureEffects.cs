using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StructureEffects", menuName = "Effect")]
public class StructureEffects : ScriptableObject
{
    [SerializeField]
    public ParticleSystem highHpEffects;
    [SerializeField]
    public ParticleSystem lowHpEffects;
    [SerializeField]
    public ParticleSystem attackedEffects;
}
