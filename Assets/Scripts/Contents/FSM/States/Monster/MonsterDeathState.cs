using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathState : MonsterBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Death;
        
    }

    public override void Enter()
    {

        Debug.Log("Monster: Daath State!!");
        Debug.Log($"Monster: {MonsterFSM.IsDead}");

        int layer = LayerMask.NameToLayer("Interactable");

        ChangeLayerRecursively(gameObject, layer);
    }

    public override void ExecuteUpdate()
    {
    }

    public override void Exit()
    {

    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void ChangeLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            ChangeLayerRecursively(child.gameObject, layer);
        }
    }
}
