using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementObject : MonoBehaviour
{
    public bool IsPlaced { get; set; }
    public PlacementData PlacementData { get; set; }
    public Vector3Int Position { get; set; }
    public bool IsCollision { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            IsCollision = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            IsCollision = false;
        }
    }
}
