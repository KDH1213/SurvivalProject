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

    public SFXPlayer Play(Transform target)
    {
        if (lastPlayIndex == maxPlayCount)
            lastPlayIndex = 0;

        sfxPlayerList[lastPlayIndex].StopSFX();
        sfxPlayerList[lastPlayIndex].transform.parent = target;
        sfxPlayerList[lastPlayIndex].transform.localPosition = Vector3.zero;
        sfxPlayerList[lastPlayIndex++].PlaySFX();

        return sfxPlayerList[lastPlayIndex - 1];
    }

    public bool CheckPool()
    {
        return sfxPlayerList.Count == maxPlayCount;
    }
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private SoundTableData soundTableData;

    private Dictionary<int, SoundPool> playSoundTable = new Dictionary<int, SoundPool>();

    [SerializeField] 
    private AudioMixer masterMixer;

    private const string masterName = "Master";
    private const string effectName = "SFX";
    private const string bgmName = "BGM";

    private float masterVolume = 0.5f;
    public bool isOnSound = true;


    protected override void Awake()
    {
        base.Awake();

        if(soundTableData == null)
        {
            soundTableData = Resources.Load<SoundTableData>("Scripteables/Sound/SoundData");
        }
    }

    private void Start()
    {
        playSoundTable.Clear();
        playSoundTable = new Dictionary<int, SoundPool>();

        if(masterMixer == null)
        {
            return;
        }

        masterMixer.SetFloat(masterName, Mathf.Log10(masterVolume) * 20);
        masterMixer.SetFloat(bgmName, Mathf.Log10(masterVolume) * 20);
        masterMixer.SetFloat(effectName, Mathf.Log10(masterVolume * 0.5f) * 20);

        
        OnSound(isOnSound);
    }

    public void OnSound(bool useSound)
    {
        isOnSound = useSound;

        if (!isOnSound)
        {
            masterMixer.SetFloat(masterName, Mathf.Log10(-80f) * 20f);
        }
        else
        {
            masterMixer.SetFloat(masterName, Mathf.Log10(masterVolume) * 20f);
        }
    }

    public void OnValueChangedMasterVolume(float volume)
    {
        masterMixer.SetFloat(masterName, Mathf.Log10(volume) * 20);
        masterMixer.SetFloat(bgmName, Mathf.Log10(volume) * 20);
        masterMixer.SetFloat(effectName, Mathf.Log10(volume * 0.5f) * 20);
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
            var soundPrefab = soundTableData.GetSFXPlayer(Id);

            if (soundPrefab != null)
            {
                playSoundTable.Add(Id, new SoundPool(Id));
                var sfx = Instantiate(soundPrefab);
                playSoundTable[Id].sfxPlayerList.Add(sfx);
                sfx.PlaySFX();
            }
        }
        else
        {
            if(playSoundTable[Id].CheckPool())
            {
                playSoundTable[Id].Play();
            }
            else
            {
                var sfx = Instantiate(soundTableData.GetSFXPlayer(Id));
                playSoundTable[Id].sfxPlayerList.Add(sfx);
                sfx.PlaySFX();
            }
        }
    }

    public SFXPlayer OnSFXPlay(Transform target, int Id)
    {
        if (!playSoundTable.ContainsKey(Id))
        {
            var soundPrefab = soundTableData.GetSFXPlayer(Id);

            if (soundPrefab != null)
            {
                playSoundTable.Add(Id, new SoundPool(Id));
                var sfx = Instantiate(soundPrefab, target);
                playSoundTable[Id].sfxPlayerList.Add(sfx);
                sfx.PlaySFX();

                return sfx;
            }
        }
        else
        {
            if (playSoundTable[Id].CheckPool())
            {
                return playSoundTable[Id].Play(target);
            }
            else
            {
                var sfx = Instantiate(soundTableData.GetSFXPlayer(Id), target);
                playSoundTable[Id].sfxPlayerList.Add(sfx);
                sfx.PlaySFX();

                return sfx;
            }
        }

        return null;
    }
}

