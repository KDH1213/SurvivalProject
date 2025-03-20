using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MapToolData", menuName = "System/MapTool/MapToolData", order = 0)]
public class MapToolData : ScriptableObject
{
    [field : SerializeField]
    public List<string> TabNameList = new List<string>();

    [SerializedDictionary]
    public SerializedDictionary<string, List<GameObject>> tabGameObjectTable = new SerializedDictionary<string, List<GameObject>>();

    //public List<Texture2D> GetPreview()
    //{
    //    var textre2DList = new List<Texture2D>();
    //    foreach (GameObject go in TestGameObjectList)
    //    {
    //        textre2DList.Add(AssetPreview.GetAssetPreview(go));
    //    }

    //    return textre2DList;
    //    // AssetPreview.GetAssetPreview
    //}

    public List<Texture2D> GetPreview(string tab)
    {
        var textre2DList = new List<Texture2D>();
        foreach (GameObject go in tabGameObjectTable[tab])
        {
            textre2DList.Add(AssetPreview.GetAssetPreview(go));
        }

        return textre2DList;
        // AssetPreview.GetAssetPreview
    }
}
