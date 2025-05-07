using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneLoader : MonoBehaviour
{
    private void Awake()
    {
        SceneLoader.Instance.StartSwitchScene(SaveLoadManager.Data.startStage);
    }
}
