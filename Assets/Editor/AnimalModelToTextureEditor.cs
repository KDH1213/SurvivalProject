using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimalModelToTexture))]
public class AnimalModelToTextureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("RenderTexture To Png"))
        {
            var gobjTex = (AnimalModelToTexture)target;
            gobjTex.StartCapture();
        }

        base.OnInspectorGUI();
    }
}
