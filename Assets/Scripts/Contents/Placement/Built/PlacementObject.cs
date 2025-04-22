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
    
    public List<ActInfo> actInfos { get; set; } = new List<ActInfo>();
    public PlacementUIController uiController { get; set; }

    public UnityEvent closeUI;
    public InteractType InteractType => InteractType.Placement;

    public bool IsInteractable => true;

    public UnityEvent<GameObject> OnEndInteractEvent => null;

    public float InteractTime => 0f;

    public abstract void SetData();

    public void CreateActInfo()
    {
        var structureData = DataTableManager.StructureTable.Get(ID);
        var data = DataTableManager.ConstructionTable.Get(structureData.PlaceBuildingID);

        actInfos.Add(new ActInfo(SurvivalStatType.Fatigue, data.PlusFatigue));
        actInfos.Add(new ActInfo(SurvivalStatType.Hunger, data.MinusSatiation));
        actInfos.Add(new ActInfo(SurvivalStatType.Thirst, data.MinusHydration));
    }
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

        if (Hp <= 0 && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            GetComponent<StructureStats>().OnDestoryStructure();
            uiController.GetComponent<PlacementSystem>().DestoryStructure(this);
        }
    }
}

