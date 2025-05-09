using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateSpriteEditor : EditorWindow
{
    [SerializeField]
    private GameObject[] createObjects;

    private readonly string path = "Assets/Resources/Sprites/Create";
    private SerializedObject serializedObject;

    private Vector2 scrollPos = Vector2.zero;

    private void OnEnable()
    {
        ScriptableObject target = this;
        serializedObject = new SerializedObject(target);
    }

    [MenuItem("Tools/CreateSpriteEditor")]
    public static void ShowWindow()
    {
        CreateSpriteEditor window = (CreateSpriteEditor)GetWindow(typeof(CreateSpriteEditor), false, "CreateSpriteEditor", true);
        window.Show();
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        GUILayout.Label("CreateSprite:");
        serializedObject.Update();
        SerializedProperty titleProperty = serializedObject.FindProperty("createObjects");
        EditorGUILayout.PropertyField(titleProperty, true);
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Button"))
        {
            GetPreview();
        }

        EditorGUILayout.EndScrollView();
    }


    [ContextMenu("CreateSprite")]
    public void GetPreview()
    {
        var textre2DList = new List<Texture2D>();
        foreach (GameObject go in createObjects)
        {
            var texture2D = AssetPreview.GetAssetPreview(go);
            ExecuteRender(texture2D, go.name);
            // var sprite = Sprite.Create(texture2D, texture2D.GetRect(), texture2D.GetRect().center);
        }
        
    }

    private void ExecuteRender(Texture2D texture2D, string filename)
    {

        // Export to the file system, if ExportFilePath is specified.
        if (texture2D != null && !string.IsNullOrWhiteSpace(filename) && !string.IsNullOrWhiteSpace(filename))
        {
            string dir = System.IO.Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            //foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            //{
            //    filename = filename.Replace(c, '_');
            //}

            string finalPath = string.Format("{0}/{1}.png", path, filename);

            byte[] bytes = texture2D.EncodeToPNG();
            System.IO.File.WriteAllBytes(filename + ".png", bytes);
        }
    }
}
