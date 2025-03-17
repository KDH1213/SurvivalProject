using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField]
    private int sfxID;

    [SerializeField]
    private string sfxName;

    [SerializeField]
    private AudioSource sfxSource;

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void PlaySFX()
    {
        gameObject.SetActive(true);
        sfxSource.Play();
    }
}
