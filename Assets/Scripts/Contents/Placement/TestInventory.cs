using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TestInventory : MonoBehaviour
{
    [SerializedDictionary("type", "count")]
    public SerializedDictionary<int, int> inventory;

    public int playerLevel = 1;

    public bool CheckItemCount(Dictionary<int, int> items)
    {
        foreach (var item in items)
        {
            if (!inventory.ContainsKey(item.Key))
            {
                return false;
            }
            if(inventory[item.Key] < item.Value)
            {
                return false;
            }
        }
        return true;
    }
    public void MinusItem(Dictionary<int, int> items)
    {
        foreach (var item in items)
        {
            if(!inventory.ContainsKey(item.Key))
            {
                continue;
            }
            inventory[item.Key] -= item.Value;
        }
    }

    public void PlusItem(Dictionary<int, int> items)
    {
        foreach (var item in items)
        {
            if (!inventory.ContainsKey(item.Key))
            {
                inventory.Add(item.Key, item.Value);
                continue;
            }
            inventory[item.Key] += Mathf.RoundToInt(item.Value * 0.2f);
        }
    }
}
