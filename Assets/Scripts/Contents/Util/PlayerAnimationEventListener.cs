using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEventListener : MonoBehaviour
{
    public UnityEvent startAttackEvent;
    public UnityEvent endAttackAnimationEvent;
    public UnityEvent createAttackVFXEvnet;

    public void OnStartAttack()
    {
        startAttackEvent?.Invoke();
    }

    public void OnEndAttackAnimation()
    {
        endAttackAnimationEvent?.Invoke();
    }

    public void OnCreateVFX()
    {
        createAttackVFXEvnet?.Invoke();
    }
}
