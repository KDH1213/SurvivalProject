using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private StageData stageData;

    [SerializeField]
    private Transform gatherTransform;
    [SerializeField]
    private Vector3 initPosition;

    // TODO :: 테스트용 프리팹
    [SerializeField]
    private GameObject tilePrefab;

    private List<GameObject> tileList = new List<GameObject>();

    private Map map;

    private void Awake()
    {
        if(map == null)
        {
            CreateTile();
        }
    }

    [ContextMenu("CreateTile")]
    private void CreateTile()
    {
        OnDestroyTile();

        map = new Map();
        map.CreateMap(stageData.MapWidth, stageData.MapHeight);

        var startPos = initPosition;
        startPos.x += stageData.TileSize.x * 0.5f;
        startPos.z -= stageData.TileSize.y * 0.5f;

        var position = startPos;
        for (int y = 0; y < stageData.MapHeight; ++y)
        {
            for (int x = 0; x < stageData.MapWidth; ++x)
            {
                var newGameObject = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                position.x += stageData.TileSize.x;
                tileList.Add(newGameObject);
            }

            position.x = startPos.x;
            position.z -= stageData.TileSize.y;
        }
    }

    [ContextMenu("CreateGather")]
    private void CreateGather()
    {
        for (int i = 0; i < (int)GatherType.End; ++i)
        {
            if(stageData.CreateGatherPercentTable.ContainsKey((GatherType)i))
            {
                if(stageData.UseCreatePercent)
                {
                    map.CreateGather((GatherType)i, stageData.CreateGatherPercentTable[(GatherType)i].GetRendomValue());
                }
                else
                {
                    map.CreateGather((GatherType)i, stageData.CreateGatherCountTable[(GatherType)i].GetRendomValue());
                }

            }
        }

        foreach (var tile in map.tiles)
        {
            if(tile.GatherType == GatherType.None || tile.GatherType == GatherType.End)
            {
                continue;
            }

            var createPosition = tileList[tile.IndexID].transform.position;
            var gatherInfo = stageData.GetRandomGatherInfo(tile.GatherType);
            createPosition += gatherInfo.offsetPosition;

            Instantiate(gatherInfo.prefab, createPosition, Quaternion.identity, gatherTransform);
        }
    }

    [ContextMenu("DestroyTile")]
    private void OnDestroyTile()
    {
        foreach(GameObject tile in tileList)
        {
#if UNITY_EDITOR
            DestroyImmediate(tile);
#else
            Destroy(gameObject);
#endif
        }

        tileList.Clear();
    }

}
