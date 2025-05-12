using UnityEngine;
using UnityEngine.UI;

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

    private void OnEnable()
    {
        masterOrigin = master.value;
        sfxOrigin = sfx.value;
        bgmOrigin = bgm.value;
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
       master.value = masterOrigin;
       sfx.value = sfxOrigin;
       bgm.value = bgmOrigin;
    }
}
