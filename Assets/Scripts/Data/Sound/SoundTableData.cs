using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "System/SoundData")]
public class SoundTableData : ScriptableObject
{
    [field: SerializeField]
    public SerializedDictionary<int, SFXPlayer> SoundTable { private set; get; } = new SerializedDictionary<int, SFXPlayer>();



    public SFXPlayer GetSFXPlayer(int id)
    {
        if(SoundTable.TryGetValue(id, out var sFXPlayer))
        {
            return sFXPlayer;
        }

        else
        {
            Debug.LogError("None Sound");
            return null;
        }
    }
}
