using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStructrue : PlacementObject
{

    public override void SetData()
    {
    }

    public override void Load()
    {
        var data = SaveLoadManager.Data.placementSaveInfoList.Find(x => x.position == Position && x.id == ID);
        Hp = data.hp;
    }
}
