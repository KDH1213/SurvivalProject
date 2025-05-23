using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameObjectToTexture))]
public class GameObjectToTextureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("RenderTexture To Png"))
        {
            var gobjTex = (GameObjectToTexture)target;
            gobjTex.StartCapture();
        }

        base.OnInspectorGUI();
    }
}
