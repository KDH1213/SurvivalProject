using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlacementObject : MonoBehaviour, ISaveLoadData, IInteractable, IAttackable
{
    public PlacementData PlacementData { get; set; }
    public Vector3Int Position { get; set; }
    public Quaternion Rotation { get; set; }
    public bool IsPlaced { get; set; } = false;
    public bool IsCollision { get; set; } = false;
    public int Rank { get; set; } = 1;
    public int ID { get; set; }
    public float Hp { get; set; }
    public PlacementUIController uiController { get; set; }

    public UnityEvent closeUI;
    public InteractType InteractType => InteractType.Placement;

    public bool IsInteractable => true;

    public UnityEvent<GameObject> OnEndInteractEvent => throw new System.NotImplementedException();

    public abstract void SetData();

    private void OnDisable()
    {
        closeUI.Invoke();
        closeUI.RemoveAllListeners();
    }

    public virtual void Save()
    {
        if(SaveLoadManager.Data == null)
        {
            return;
        }

        var saveInfo = new PlacementSaveInfo();
        saveInfo.hp = Hp;
        saveInfo.position = Position;
        saveInfo.rotation = Rotation;
        saveInfo.id = ID;
        SaveLoadManager.Data.placementSaveInfoList.Add(saveInfo);
    }

    public virtual void Load()
    {
    }

    public virtual void Interact(GameObject interactor)
    {
        
    }

    public virtual void OnAttack(GameObject attacker, DamageInfo damageInfo)
    {
        Hp -= damageInfo.damage;

        if (Hp <= 0)
        {
            gameObject.SetActive(false);
            GetComponent<StructureStats>().OnDestoryStructure();
            uiController.GetComponent<PlacementSystem>().DestoryStructure(this);
        }
    }
}

