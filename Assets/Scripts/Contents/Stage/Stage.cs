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

    private List<GameObject> tileList = new List<GameObject>();
    private List<GameObject> gatherList = new List<GameObject>();

    private bool isReset = true;
    private float resetTime;

    private Map map;

    //private void Awake()
    //{
    //    if (map == null)
    //    {
    //        CreateTile();
    //    }

    //    if(isReset)
    //    {
    //        OnDestroyGather();
    //        CreateGather();
    //    }
    //}

    // TODO :: 추 후 세이브 로드 추가 시 삭제 예정
    private void Update()
    {
        ResetStage();
    }

    private void ResetStage()
    {
        if (!isReset && resetTime < Time.time)
        {
            OnDestroyGather();
            CreateGather();
        }
    }

    [ContextMenu("타일 생성")]
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
                var newGameObject = Instantiate(stageData.GetRandomTile(), position, Quaternion.identity, transform);
                position.x += stageData.TileSize.x;
                tileList.Add(newGameObject);
            }

            position.x = startPos.x;
            position.z -= stageData.TileSize.y;
        }
    }

    [ContextMenu("상호작용 오브젝트 생성")]
    private void CreateGather()
    {
        if(tileList.Count == 0)
        {
            return;
        }

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
                    var count = stageData.CreateGatherCountTable[(GatherType)i].GetRendomValue();
                    map.CreateGather((GatherType)i, count);
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

            if (gatherInfo.prefab.GetComponent<IGather>() == null)
            {
                gatherInfo.prefab.AddComponent<Gather>();
            }

            var newGather = Instantiate(gatherInfo.prefab, createPosition, Quaternion.identity, gatherTransform);
            var gather = newGather.GetComponent<IGather>();
            gatherList.Add(newGather);

            gather.TileID = tile.IndexID;
            gather.OnEndInteractionEvent.AddListener(map.OnChangeGatherTypeToNone);
        }

        isReset = false;
        resetTime = Time.time + stageData.ResetTime;
    }

    [ContextMenu("타일 삭제")]
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

        var childrens = transform.GetComponentsInChildren<Transform>();
        foreach (var child in childrens)
        {

#if UNITY_EDITOR
            if (child.gameObject != gameObject)
            {
                DestroyImmediate(child.gameObject);
            }
#else
            Destroy(gameObject);
#endif
        }

        tileList.Clear();
    }

    private void OnDestroyGather()
    {
        int count = gatherList.Count;

        for (int i = 0; i < count; ++i)
        {
            var gather = gatherList[i].GetComponent<IGather>();
            gather.OnEndInteractionEvent?.Invoke(gather.TileID);
            if (Application.isPlaying)
            {
                Destroy(gatherList[i]);
            }
            else
            {
                Destroy(gatherList[i]);
            }
        }
        gatherList.Clear();
    }
}
