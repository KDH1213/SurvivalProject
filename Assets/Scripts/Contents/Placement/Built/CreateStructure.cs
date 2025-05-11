using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStructure : PlacementObject
{
    public override void SetData()
    {
        
    }
    public override void Interact(GameObject interactor)
    {
        uiController.OnOpenCreateItemUI(interactor, this);
    }
}
