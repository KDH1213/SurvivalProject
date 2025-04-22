using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader>
{
    public UnityEvent onStartLoadSceneEvent;


    public string loadSceneAddress = "Assets/Scenes/LoadScene.unity";
    [SerializeField]
    private string NextSceneAddress;
    private bool isLoadScene = false;
    private bool isChangeScene = false;

    private AsyncOperationHandle<SceneInstance> sceneOperation;


    private void OnDestroy()
    {
        // Addressables.Release(sceneOperation);
    }

    public void SwitchScene(string nextScene, bool useFade = true)
    {
        if(isChangeScene)
        {
            return;
        }

        isChangeScene = true;
        NextSceneAddress = nextScene;
        if (useFade)
        {
            LoadScene(loadSceneAddress);
            //FadeController.Instance.FadeIn(() =>
            //{
            //    LoadScene(loadSceneAddress);
            //});
        }
        else
        {
            LoadScene(loadSceneAddress);
        }
    }

    private void LoadScene(string nextScene)
    {
        StartCoroutine(CoInitAddressables(nextScene));
    }

    private void LoadNextScene(string nextScene)
    {
        StartCoroutine(CoInitAddressables(nextScene, false));
    }

    private IEnumerator CoInitAddressables(string nextScene, bool loadScene = true)
    {
        var initializeAsync = Addressables.InitializeAsync();
        yield return initializeAsync;


        Slider loadingBar = null;
        loadingBar = FindFirstObjectByType<Slider>();

        sceneOperation = Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Single, false);

        while (!sceneOperation.IsDone)
        {
            Debug.Log(sceneOperation.PercentComplete * 100f);
            if (loadingBar != null)
            {
                loadingBar.value = sceneOperation.PercentComplete;
            }

            yield return null;
        }

        if (loadingBar != null)
        {
            loadingBar.value = sceneOperation.PercentComplete;
        }

        sceneOperation.Result.ActivateAsync();


        if (loadScene)
        {
            LoadNextScene(NextSceneAddress);
        }
        else
        {
            isChangeScene  = false;
        }
        Addressables.Release(sceneOperation);
    }

    public void OnLoad()
    {
        if (!isLoadScene)
        {
            // Addressables.LoadSceneAsync(NextSceneAddress);
            SwitchScene(NextSceneAddress);

            // StartCoroutine(CoInitAddressables(NextSceneAddress));
            onStartLoadSceneEvent?.Invoke();
            isLoadScene = true;
        }
    }
}
