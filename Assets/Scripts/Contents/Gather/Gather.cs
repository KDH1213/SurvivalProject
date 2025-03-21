using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gather : MonoBehaviour, IGather
{
    public UnityEvent<int> OnEndInteractionEvent { get; set; }
    public int TileID { get; set; }

    public void OnInteraction()
    {
    }
     
    public void OnEndInteraction()
    {
        OnEndInteractionEvent?.Invoke(TileID);
    }
}
