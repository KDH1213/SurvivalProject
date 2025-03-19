using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map
{
    public Tile[] tiles;

    public Tile[] NoneTile => tiles.Where(x => x.GatherType == GatherType.None).ToArray();

    private int mapWdith;
    private int mapHeight;

    public void CreateMap(int width, int height)
    {
        mapWdith = width;
        mapHeight = height;

        tiles = new Tile[mapHeight * mapWdith];

        for (int i = 0; i < tiles.Length; ++i)
        {
            tiles[i] = new Tile();
            tiles[i].SetTileIndex(i);
        }
    }

    public void CreateGather(GatherType gatherType, float percent)
    {
        var noneTile = NoneTile;
        SuffleTiles(noneTile);
        int totalCount = Mathf.FloorToInt(tiles.Length * percent);

        for (int i = 0; i < totalCount; ++i)
        {
            noneTile[i].SetGatherType(gatherType);
        }
    }

    public void CreateGather(GatherType gatherType, int count)
    {
        var noneTile = NoneTile;
        SuffleTiles(noneTile);

        for (int i = 0; i < count; ++i)
        {
            noneTile[i].SetGatherType(gatherType);
        }
    }

    public void SuffleTiles(Tile[] tiles)
    {
        for (int i = tiles.Length - 1; i >= 0; --i)
        {
            int rand = Random.Range(0, i + 1);

            Tile tile = tiles[i];
            tiles[i] = tiles[rand];
            tiles[rand] = tile;
        }
    }
}
