using AYellowpaper.SerializedCollections;
using UnityEngine;

public enum StructureEffectKind
{
    HighHp,
    LowHp,
    Attacked
}

public class StructurePlayEffet : MonoBehaviour
{
    [SerializedDictionary, SerializeField]
    private SerializedDictionary<StructureEffectKind, ParticleSystem> effects;
    [SerializeField]
    private PlacementObject structure;
    private int maxHp;
    private void OnEnable()
    {
        foreach (var effect in effects)
        {
            if(effect.Value == null)
            {
                continue;
            }
            effect.Value.Stop();
            effect.Value.gameObject.SetActive(false);
        }
        if (structure == null || !structure.IsPlaced)
        {
            return;
        }
    }

    private void Update()
    {
        if (structure == null || !structure.IsPlaced)
        {
            return;
        }
        int maxHp = DataTableManager.StructureTable.Get(structure.ID).BuildingHealth;
        if (structure.Hp / maxHp < 0.2)
        {
            if (effects[StructureEffectKind.LowHp].isPlaying)
            {
                return;
            }
            StopFire(StructureEffectKind.HighHp);
            PlayFire(StructureEffectKind.LowHp);
        } 
        else if (structure.Hp / maxHp < 0.7)
        {
            if (effects[StructureEffectKind.HighHp].isPlaying)
            {
                return;
            }
            StopFire(StructureEffectKind.LowHp);
            PlayFire(StructureEffectKind.HighHp);
        }
    }

    private void PlayFire(StructureEffectKind id)
    {
        effects[id].gameObject.SetActive(true);
        effects[id].Play();
    }

    private void StopFire(StructureEffectKind id)
    {
        effects[id].gameObject.SetActive(false);
        effects[id].Stop();
    }

    public void AttackedPlay()
    {

    }

}
