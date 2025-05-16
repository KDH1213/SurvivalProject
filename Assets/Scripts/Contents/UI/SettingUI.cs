using UnityEngine;
using UnityEngine.UI;

public class SettingData
{
    public float masterValue;
    public float sfxValue;
    public float bgmValue;
}
public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private Slider master;
    [SerializeField]
    private Slider sfx;
    [SerializeField]
    private Slider bgm;

    private float masterOrigin;
    private float sfxOrigin;
    private float bgmOrigin;
    private float timeScaleOrigin;

    private void OnEnable()
    {
        masterOrigin = master.value;
        sfxOrigin = sfx.value;
        bgmOrigin = bgm.value;
        timeScaleOrigin = Time.timeScale;
        Time.timeScale = 0;
        Load();
    }

    public void OnValueChangeMaster()
    {
        SoundManager.Instance.OnValueChangedMasterVolume(master.value);
    }

    public void OnValueChangeSFX()
    {
        SoundManager.Instance.OnValueChangedEffectVolume(sfx.value);
    }

    public void OnValueChangeBGM()
    {
        SoundManager.Instance.OnValueBGMEffectVolume(bgm.value);
    }

    public void OnClickCancle()
    {
        Time.timeScale = timeScaleOrigin;
        gameObject.SetActive(false);
        Save();
        Application.Quit();
    }
    public void OnClickOK()
    {
        Time.timeScale = timeScaleOrigin;
        gameObject.SetActive(false);
        Save();
    }

    public void Save()
    {
        SettingData settingInfo = new SettingData();
        settingInfo.masterValue =  master.value;
        settingInfo.sfxValue =  sfx.value;
        settingInfo.bgmValue =  bgm.value;

        SaveLoadManager.Data.settingInfo = settingInfo;
    }

    public void Load()
    {
        var data = SaveLoadManager.Data.settingInfo;
        master.value = data.masterValue;
        sfx.value = data.sfxValue;
        bgm.value = data.bgmValue;
    }
}
