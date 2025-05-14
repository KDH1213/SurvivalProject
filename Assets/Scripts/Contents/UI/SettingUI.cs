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
    private float timeScaleOrigin;

    private void OnEnable()
    {
        masterOrigin = master.value;
        sfxOrigin = sfx.value;
        bgmOrigin = bgm.value;
        timeScaleOrigin = Time.timeScale;
        Time.timeScale = 0;
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
        Application.Quit();
    }
    public void OnClickOK()
    {
        Time.timeScale = timeScaleOrigin;
        gameObject.SetActive(false);
    }
}
