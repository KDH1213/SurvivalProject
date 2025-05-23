using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
public class AnimalModelToTexture : MonoBehaviour
{
    public Camera TargetCamera;
    public List<GameObject> GameObjectToTextureQueue = new();
    private Queue<GameObject> textureQueue = new();

    public string icontexturePath = "Assets/Resources/Textures/PlayerIcon/PlayerFace/{0}Face";

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
            EditorApplication.update += ProcessQueue2;
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
        gobj.SetActive(false);
        DestroyImmediate(gobj);
    }

    private GameObject currentObj = null;
    private int frameWait = 0;

    private void ProcessQueue2()
    {
        if (currentObj == null)
        {
            if (textureQueue.Count == 0)
            {
                isProcessing = false;
                EditorApplication.update -= ProcessQueue2;
                AssetDatabase.Refresh();
                return;
            }

            currentObj = textureQueue.Dequeue();
            currentObj.SetActive(true);


            frameWait = 10; 
            return;
        }

        if (frameWait > 0)
        {
            frameWait--;
            return;
        }

        SceneView.RepaintAll();

        TargetCamera.Render();
        SaveTextureToFileUtility.SaveRenderTextureToFile(TargetCamera.targetTexture, string.Format(icontexturePath, currentObj.name));

        Debug.Log("Mug shot : " + currentObj.name);
        currentObj.SetActive(false);
        DestroyImmediate(currentObj);
        currentObj = null;
    }

    public void Test()
    {
        SaveTextureToFileUtility.SaveRenderTextureToFile(TargetCamera.targetTexture, string.Format(icontexturePath, "temp"));
    }
}

#endif
