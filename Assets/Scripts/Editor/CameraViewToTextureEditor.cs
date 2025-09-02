using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraViewToTextureEditor : EditorWindow
{
    private Camera camera;
    private static string texturePath = "Assets/Resources/Textures";

    private Camera copyTargetCamera;
    private CameraRenderToTexture test;
    [MenuItem("Tools/CameraViewToTextureEditor")]
    private static void Open()
    {
        CameraViewToTextureEditor window = (CameraViewToTextureEditor)GetWindow(typeof(CameraViewToTextureEditor), false, "CameraViewToTextureEditor", true);
        window.Show();
    }

    private void OnEnable()
    {
        var createGameObject = new GameObject("CameraViewToTextureCamera");
        camera = createGameObject.AddComponent<Camera>();

        Undo.AddComponent<CameraRenderToTexture>(camera.gameObject);
    }

    private void OnDisable()
    {
        DestroyImmediate(camera.gameObject);
        camera = null;
    }

    private void OnGUI()
    {
        EditorGUIUtility.labelWidth = 200;
        // EditorGUI.indentLevel++;

        copyTargetCamera = (Camera)EditorGUILayout.ObjectField("CopyTargetCamera", copyTargetCamera, typeof(Camera), true);
        if (GUILayout.Button("¼¼ÆÃ º¹»ç"))
        {
            if(copyTargetCamera != null)
            {
                camera.CopyFrom(copyTargetCamera);
            }
        }
        EditorGUILayout.Space(10);


        texturePath = EditorGUILayout.TextField(texturePath);
        if (GUILayout.Button("°æ·Î ÁöÁ¤"))
        {
            OpenFilePanel();
        }

        if (GUILayout.Button("¾À ºä Ä¸ÃÄ"))
        {
            if(camera != null)
            {
                RenderTexture rt = new RenderTexture(512, 512, 24);
                camera.targetTexture = rt;

                camera.Render();
                SaveTextureToFileUtility.SaveRenderTextureToFile(camera.targetTexture, string.Format(texturePath, "temp"));
                camera.targetTexture = null;
                rt.Release();

            }
        }

    }

    public void OpenFilePanel()
    {
        string path = EditorUtility.OpenFolderPanel("Load Scene", "Assets", "unity");
        if (!string.IsNullOrEmpty(path))
        {
            texturePath = path;
        }
    }


}
