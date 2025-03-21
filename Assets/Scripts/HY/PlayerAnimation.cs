using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static readonly int hashWalk = Animator.StringToHash("walk");

    private Animator animator;
    private PlayerCurrentState playerCurrentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerCurrentState = GetComponent<PlayerCurrentState>();
    }

    private void Update()
    {
        Animation();
    }

    private void Animation()
    {
        if(playerCurrentState.CurrentPlayerState == CharactorStateType.Move)
        {
            animator.SetBool(hashWalk, true);
        }
        else if(playerCurrentState.CurrentPlayerState == CharactorStateType.Idle)
        {
            animator.SetBool(hashWalk, false);
        }
        else if(playerCurrentState.CurrentPlayerState == CharactorStateType.Attack)
        {

        }
    }
}
