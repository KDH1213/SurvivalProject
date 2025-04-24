using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ArmorMeshData", menuName = "System/Player/ArmorMeshData", order = 1)]
public class ArmorMeshData : ScriptableObject
{
    [SerializeField]
    private AdvancedPeopleSystem.CharacterSettings characterSettings;

    [field: SerializeField]
    public SerializedDictionary<int, List<Mesh>> HelmetMeshTable { get; private set; } = new SerializedDictionary<int, List<Mesh>>();

    [field: SerializeField]
    public SerializedDictionary<int, List<Mesh>> ArmorMeshTable = new SerializedDictionary<int, List<Mesh>>();


    [field: SerializeField]
    public SerializedDictionary<int, List<Mesh>> PantsMeshTable { get; private set; } = new SerializedDictionary<int, List<Mesh>>();


    [field: SerializeField]
    public SerializedDictionary<int, List<Mesh>> ShoesMeshTable { get; private set; } = new SerializedDictionary<int, List<Mesh>>();

    [field: SerializeField]
    public List<SerializedDictionary<int, List<Mesh>>> ArmorMeshTableList { get; private set; } = new List<SerializedDictionary<int, List<Mesh>>>();

    [field: SerializeField]
    public SerializedDictionary<ArmorType, List<Mesh>> defalutTable { get; private set; } = new SerializedDictionary<ArmorType, List<Mesh>>();

    public List<Mesh> GetMeshList(ArmorType armorType, int id)
    {
        if (ArmorMeshTableList.Count == 0)
        {
            Initialize();
        }

        int index = (int)armorType - 1;

        if (ArmorMeshTableList[index].TryGetValue(id, out var meshList))
        {
            return meshList;
        }
        else
        {
            // Debug.LogError($"{id} : Key에 해당한 Mesh가 없음");
            return null;
        }
    }

    [ContextMenu("Initialize")]
    private void Initialize()
    {
        ArmorMeshTableList.Clear();
        ArmorMeshTableList.Add(HelmetMeshTable);
        ArmorMeshTableList.Add(ArmorMeshTable);
        ArmorMeshTableList.Add(PantsMeshTable);
        ArmorMeshTableList.Add(ShoesMeshTable);

        //var list = characterSettings.hatsPresets;

        //HelmetMeshTable.Clear();
        //int count = 0;

        //foreach (var item in list)
        //{
        //    List<Mesh> meshList = new List<Mesh>();

        //    for (int i = 0; i < item.mesh.Length; i++)
        //    {
        //        meshList.Add(item.mesh[i]);
        //    }
        //    HelmetMeshTable.Add(count++, meshList);
        //}

        //list = characterSettings.shirtsPresets;
        //ArmorMeshTable.Clear();
        //count = 0;

        //foreach (var item in list)
        //{
        //    List<Mesh> meshList = new List<Mesh>();

        //    for (int i = 0; i < item.mesh.Length; i++)
        //    {
        //        meshList.Add(item.mesh[i]);
        //    }
        //    ArmorMeshTable.Add(count++, meshList);
        //}

        //list =characterSettings.pantsPresets;
        //PantsMeshTable.Clear();
        //count = 0;

        //foreach (var item in list)
        //{
        //    List<Mesh> meshList = new List<Mesh>();

        //    for (int i = 0; i < item.mesh.Length; i++)
        //    {
        //        meshList.Add(item.mesh[i]);
        //    }
        //    PantsMeshTable.Add(count++, meshList);
        //}

        //list= characterSettings.shoesPresets;
        //ShoesMeshTable.Clear();
        //count = 0;

        //foreach (var item in list)
        //{
        //    List<Mesh> meshList = new List<Mesh>();

        //    for (int i = 0; i < item.mesh.Length; i++)
        //    {
        //        meshList.Add(item.mesh[i]);
        //    }
        //    ShoesMeshTable.Add(count++, meshList);
        //}
    }
}
