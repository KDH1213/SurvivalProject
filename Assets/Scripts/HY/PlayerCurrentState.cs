using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrentState : MonoBehaviour
{
    public CharactorStateType CurrentPlayerState { get; set; }

    private void Awake()
    {
        CurrentPlayerState = CharactorStateType.Idle;
    }
}
