using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathState : MonsterBaseState
{
    //[SerializeField]
    //private Inventory inventory;

    // public List<ItemData> items;

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Death;
    }

    public override void Enter()
    {
        // TODO :: 현재 체력 0이 되면 오브젝트 삭제
        // Destroy(gameObject);
        MonsterFSM.OnEndInteractEvent?.Invoke(gameObject);

        //for (int i = 0; i < items.Count; i++)
        //{
        //    ItemData newItem = Instantiate(items[i]);

        //    Debug.Log(newItem.name);

        //    inventory.AddItem(newItem);
        //}

        gameObject.SetActive(false);
    }

    public override void ExecuteUpdate()
    {
    }

    public override void Exit()
    {

    }
}
