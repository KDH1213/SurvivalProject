using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ParticleSystemCallbackListener : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    public UnityEvent endEvent;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();

        var psMain = particleSystem.main;
        psMain.stopAction = ParticleSystemStopAction.Callback;
    }

    public void OnParticleSystemStopped()
    {
        endEvent?.Invoke();
    }
}
