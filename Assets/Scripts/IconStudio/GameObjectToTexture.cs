using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
public class GameObjectToTexture : MonoBehaviour
{
    public Camera TargetCamera;
    public List<GameObject> GameObjectToTextureQueue = new();
    private Queue<GameObject> textureQueue = new();

    public string icontexturePath = "Assets/Resources/Textures/PlayerIcon/{0}";

    private bool isProcessing = false;

    public void PushToGameObjectToTextureQueue(GameObject gobj)
    {
        GameObjectToTextureQueue.Add(gobj);
    }

    public void StartCapture()
    {
        foreach (var gobj in GameObjectToTextureQueue)
        {
            var target = Instantiate(gobj);
            target.name = gobj.name;
            target.transform.position = Vector3.zero;
            target.SetActive(false);
            textureQueue.Enqueue(target);
        }

        if (!isProcessing)
        {
            isProcessing = true;
            EditorApplication.update += ProcessQueue;
        }
    }

    private void ProcessQueue()
    {
        if (textureQueue.Count == 0)
        {
            isProcessing = false;
            EditorApplication.update -= ProcessQueue;
            AssetDatabase.Refresh();
            return;
        }

        var gobj = textureQueue.Dequeue();
        gobj.SetActive(true);

        SceneView.RepaintAll();


        TargetCamera.Render();
        SaveTextureToFileUtility.SaveRenderTextureToFile(TargetCamera.targetTexture, string.Format(icontexturePath, gobj.name));

        Debug.Log("Mug shot : " + gobj.name);
        DestroyImmediate(gobj);
    }
    public void Test()
    {
        SaveTextureToFileUtility.SaveRenderTextureToFile(TargetCamera.targetTexture, string.Format(icontexturePath, "temp"));
    }
}

#endif
