using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;

public class GameTimeManager : Singleton<GameTimeManager>, ISaveLoadData
{
    [SerializeField] 
    private TextMeshProUGUI timeText;

    [SerializeField] 
    private Light sun;
    [SerializeField] 
    private Light moon;
    [SerializeField] 
    private AnimationCurve lightIntensityCurve;
    [SerializeField] 
    private float maxSunIntensity = 1;
    [SerializeField] 
    private float maxMoonIntensity = 0.5f;

    [SerializeField] 
    private Color dayAmbientLight;
    [SerializeField] 
    private Color nightAmbientLight;
    [SerializeField] 
    private Volume volume;
    [SerializeField] 
    private Material skyboxMaterial;

    [SerializeField] 
    private RectTransform dial;

    [SerializeField] 
    private TimeSettings timeSettings;

    private ColorAdjustments colorAdjustments;
    float initialDialRotation;

    public event Action OnSunrise
    {
        add => service.OnSunrise += value;
        remove => service.OnSunrise -= value;
    }

    public event Action OnSunset
    {
        add => service.OnSunset += value;
        remove => service.OnSunset -= value;
    }

    public event Action OnHourChange
    {
        add => service.OnHourChange += value;
        remove => service.OnHourChange -= value;
    }

    TimeService service;

    protected override void Awake()
    {
        base.Awake();

        Load();
    }

    private void Start()
    {

        if(service == null)
        {
            service = new TimeService(timeSettings);
        }
        
        volume.profile.TryGet(out colorAdjustments);

        // initialDialRotation = dial.rotation.eulerAngles.z;
    }

    private void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
        //UpdateSkyBlend();
    }

    private void UpdateSkyBlend()
    {
        float dotProduct = Vector3.Dot(sun.transform.forward, Vector3.up);
        float blend = Mathf.Lerp(0, 1, lightIntensityCurve.Evaluate(dotProduct));
        // skyboxMaterial.SetFloat("_Blend", blend);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sun.transform.forward, Vector3.down);
        float lightIntensity = lightIntensityCurve.Evaluate(dotProduct);

        sun.intensity = Mathf.Lerp(0, maxSunIntensity, lightIntensity);
        moon.intensity = Mathf.Lerp(maxMoonIntensity, 0, lightIntensity);

        if (colorAdjustments == null) 
            return;
        colorAdjustments.colorFilter.value = Color.Lerp(nightAmbientLight, dayAmbientLight, lightIntensity);
    }

    private void RotateSun()
    {
        float rotation = service.CalculateSunAngle();
        sun.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);
        // dial.rotation = Quaternion.Euler(0, 0, rotation + initialDialRotation);
    }

    private void UpdateTimeOfDay()
    {
        service.UpdateTime(Time.deltaTime);
        if (timeText != null)
        {
            timeText.text = service.CurrentTime.ToString("HH:mm");
        }
    }

    public void Save()
    {
        SaveLoadManager.Data.gameTime = service.CurrentTime;
    }

    public void Load()
    {
        var data = SaveLoadManager.Data;
        if (data != null && !data.isRestart && data.gameTime.Millisecond != 0)
        {
            service = new TimeService(timeSettings, SaveLoadManager.Data.gameTime);
        }
    }
}
