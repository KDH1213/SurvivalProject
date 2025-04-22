using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestStructure : PlacementObject
{
    [SerializeField]
    private float recoverHp;
    [SerializeField]
    private float recoverThirsty;
    [SerializeField]
    private float recoverHunger;
    [SerializeField]
    private float recoverFatigue; 
    [SerializeField]
    private float recoverTemperature;

    private GameObject target;
    private bool isRest = false;
    [SerializeField]
    private float timeScale;

    private void Update()
    {
        if(!isRest)
        {
            return;
        }
    }

    public override void SetData()
    {
        
    }

    public override void Interact(GameObject interactor)
    {
        isRest = true;
        Time.timeScale *= timeScale;
        target = interactor;
    }
}
