using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStructure : PlacementObject
{
    public override void SetData()
    {
        
    }

    public override void Load()
    {
        var data = SaveLoadManager.Data.placementSaveInfoList.Find(x => x.position == Position && x.id == ID);
        Hp = data.hp;
    }

    public override void Interact(GameObject interactor)
    {
        uiController.OnOpenCreateItemUI(interactor, this);
    }
}
