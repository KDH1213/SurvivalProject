using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveLoadFileWindow : EditorWindow
{
    [MenuItem("Tools/SaveLoad File Window")]
    public static void ShowWindow()
    {
        GetWindow<SaveLoadFileWindow>();
    }

    void OnGUI()
    {
        GUILayout.Label("Save File");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Remove CurrentSaveFile");
        if (GUILayout.Button("Remove"))
        {
            var path = Path.Combine(SaveLoadManager.SaveDirectory, SaveLoadManager.DefaultSaveFileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Open CurrentSaveFile");
        if (GUILayout.Button("Open"))
        {
            var path = Path.Combine(SaveLoadManager.SaveDirectory, SaveLoadManager.DefaultSaveFileName);
            try
            {
                Process.Start(path); // 파일을 열거나 실행
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                UnityEngine.Debug.Log("파일을 열거나 실행할 수 없습니다: " + e.Message);
            }
        }
        GUILayout.EndHorizontal();

    }
}