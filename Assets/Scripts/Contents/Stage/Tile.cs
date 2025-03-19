using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public GatherType GatherType { get; private set; } = GatherType.None;
    public int IndexID { get; private set; }

    public bool CanVisit { get; private set; }

    public void SetGatherType(GatherType type)
    {
        GatherType = type;
    }

    public void SetTileIndex(int index)
    {
        IndexID = index;
    }
}
