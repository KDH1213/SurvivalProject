using UnityEngine;
using UnityEngine.UI;

public class SettingData
{
    public float masterValue = 0f;
    public float sfxValue = 0f;
    public float bgmValue = 0f;
    public int targetFrameRate = 144;


}
public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private Slider master;
    [SerializeField]
    private Slider sfx;
    [SerializeField]
    private Slider bgm;

    [SerializeField]
    private Toggle[] toggles;

    private float masterOrigin;
    private float sfxOrigin;
    private float bgmOrigin;
    private float timeScaleOrigin;

    public void Initialize()
    {
        Load();
        ToggleInitialize();
    }

    private void OnEnable()
    {
        masterOrigin = master.value;
        sfxOrigin = sfx.value;
        bgmOrigin = bgm.value;
        timeScaleOrigin = Time.timeScale;
        Time.timeScale = 0;
        Load();
    }

    private void ToggleInitialize()
    {
        var targetFrameRate = SaveLoadManager.Data.settingInfo.targetFrameRate;
        var index = 0;

        if (targetFrameRate == 30)
        {
            index = 0;
        }
        else if (targetFrameRate == 60)
        {
            index = 1;
        }
        else
        {
            index = 2;
        }

        for (int i = 0; i < toggles.Length; ++i)
        {
            if(index == i)
            {
                toggles[i].isOn = true;
            }
            else
            {
                toggles[i].isOn = false;
            }

            toggles[i].onValueChanged.AddListener(OnValueChangeFrame);
        }       
    }

    private void OnValueChangeFrame(bool value)
    {
        if(toggles[0].isOn)
        {
            Application.targetFrameRate = 30;
        }

        if (toggles[1].isOn)
        {
            Application.targetFrameRate = 60;
        }

        if (toggles[2].isOn)
        {
            Application.targetFrameRate = 144;
        }

        Save();
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
        settingInfo.targetFrameRate =  Application.targetFrameRate;

        SaveLoadManager.Data.settingInfo = settingInfo;
    }

    public void Load()
    {
        var data = SaveLoadManager.Data.settingInfo;
        master.value = data.masterValue;
        sfx.value = data.sfxValue;
        bgm.value = data.bgmValue;
        Application.targetFrameRate = data.targetFrameRate;
    }
}
