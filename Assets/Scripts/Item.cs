using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item.asset", menuName = "Util/BaseItem")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
}
