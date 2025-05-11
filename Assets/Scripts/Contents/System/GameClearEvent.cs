using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameClearEvent : MonoBehaviour
{
    public UnityEvent onStageClearEvent;

    public void OnStageClear()
    {
        onStageClearEvent?.Invoke();
    }

}
