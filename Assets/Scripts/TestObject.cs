using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TestObject : MonoBehaviour
{
    private PlayerFSM playerFSM;

    public int Hp { get; private set; }

    private void Awake()
    {
        var playerObject = GameObject.FindWithTag("Player");
        playerFSM = playerObject.GetComponent<PlayerFSM>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger Enter: {other.name}");

        if (other.CompareTag("Player"))
        {
            playerFSM.OnSetIsPlayerInRange(true);
            playerFSM.target = gameObject;
            Debug.Log("Can Interact!!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerFSM.OnSetIsPlayerInRange(false);
            playerFSM.target = null;
            Debug.Log("Can't Interact!!");
        }
    }

    public void Damage()
    {
        Hp--;
        Debug.Log($"{Hp}");

        if(Hp == 0)
        {
            OnDestroy();
        }
    }

    private void OnDestroy()
    {
        playerFSM.target = null;
        Destroy(gameObject);
    }
}
