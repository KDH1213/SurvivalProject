using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalOption : MonoBehaviour
{
    [SerializeField]
    private Toggle fpsToggle;

    private bool isOnFPS = false;

    [SerializeField]
    private int targetFrameRate;

    private GameModeType currentMode = GameModeType.GamePlay;   

    public static float prevTimeScale;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        prevTimeScale = Time.timeScale;

#if UNITY_EDITOR
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
        UnityEngine.Rendering.DebugManager.instance.displayRuntimeUI = false;
#endif

        if (fpsToggle != null)
        {
            fpsToggle.onValueChanged.AddListener(OnFPS);
            fpsToggle.isOn = isOnFPS;
        }
    }

    public void OnPause()
    {
        if (currentMode == GameModeType.GamePause)
        {
            Time.timeScale = 0f;
            return;
        }

        currentMode= GameModeType.GamePause;
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void OnRestart()
    {
        currentMode= GameModeType.GamePlay;
        Time.timeScale = prevTimeScale;
    }

    public void OnExitGame()
    {
        Application.Quit();
    }

    public void OnFPS(bool value)
    {
        isOnFPS = value;
    }

    [Range(10, 150)]
    public int fontSize = 30;
    public Color color = new Color(.0f, .0f, .0f, 1.0f);
    public float width, height;

    void OnGUI()
    {
        if (!isOnFPS)
            return;
        string text;
        Rect position = new Rect(width, height, Screen.width, Screen.height);

        if(Time.timeScale == 0)
        {
            float fps = 1.0f / Time.unscaledDeltaTime;
            float ms = Time.unscaledDeltaTime * 1000.0f;
            text = string.Format("{0:N1} FPS ({1:N1}ms)", fps, ms);
        }
        else
        {
            float fps = 1.0f / Time.deltaTime;
            float ms = Time.deltaTime * 1000.0f;
            text = string.Format("{0:N1} FPS ({1:N1}ms)", fps, ms);
        }
        

        GUIStyle style = new GUIStyle();

        style.fontSize = fontSize;
        style.normal.textColor = color;

        GUI.Label(position, text, style);
    }
}
