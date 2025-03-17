using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundPool
{
    [SerializeField]
    public List<SFXPlayer> sfxPlayerList = new List<SFXPlayer>();

    private int lastPlayIndex = 0;
    public int maxPlayCount = 5;
    public int Id;

    public SoundPool(int id)
    { 
        this.Id = id; 
    }

    public void Play()
    {
        if (lastPlayIndex == maxPlayCount)
            lastPlayIndex = 0;

        sfxPlayerList[lastPlayIndex].StopSFX();
        sfxPlayerList[lastPlayIndex++].PlaySFX();
    }

    public bool CheckPool()
    {
        return sfxPlayerList.Count == maxPlayCount;
    }
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializedDictionary, SerializeField]
    private SerializedDictionary<int, SFXPlayer> soundTable;

    private Dictionary<int, SoundPool> playSoundTable = new Dictionary<int, SoundPool>();

    [SerializeField] 
    private AudioMixer masterMixer;

    private const string masterName = "Master";
    private const string effectName = "SFX";
    private const string bgmName = "BGM";

    private float masterVolume = 0.5f;
    public bool isOnSound = true;


    private void Start()
    {
        playSoundTable.Clear();
        playSoundTable = new Dictionary<int, SoundPool>();

        masterMixer.SetFloat(masterName, Mathf.Log10(masterVolume) * 20);
        masterMixer.SetFloat(bgmName, Mathf.Log10(masterVolume) * 20);
        masterMixer.SetFloat(effectName, Mathf.Log10(masterVolume * 0.5f) * 20);

        
        OnSound(isOnSound);
    }

    public void OnSound(bool useSound)
    {
        isOnSound = useSound;

        if (!isOnSound)
            masterMixer.SetFloat(masterName, Mathf.Log10(-80f) * 20f);
        else
            masterMixer.SetFloat(masterName, Mathf.Log10(masterVolume) * 20f);
    }

    public void OnValueChangedEffectVolume(float volume)
    {
        masterMixer.SetFloat(effectName, Mathf.Log10(volume) * 20f);
    }
    public void OnValueBGMEffectVolume(float volume)
    {
        masterMixer.SetFloat(bgmName, Mathf.Log10(volume) * 20f);
    }

    public void OnSFXPlay(int Id)
    {
        if(!playSoundTable.ContainsKey(Id))
        {
            playSoundTable.Add(Id, new SoundPool(Id));
            var sfx = Instantiate(soundTable[Id]);
            playSoundTable[Id].sfxPlayerList.Add(sfx);
            sfx.PlaySFX();
        }
        else
        {
            if(playSoundTable[Id].CheckPool())
            {
                playSoundTable[Id].Play();
            }
            else
            {
                var sfx = Instantiate(soundTable[Id]);
                playSoundTable[Id].sfxPlayerList.Add(sfx);
                sfx.PlaySFX();
            }
        }
    }
}

