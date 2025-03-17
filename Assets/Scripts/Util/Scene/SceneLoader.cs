using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public UnityEvent onStartLoadSceneEvent;

    [SerializeField]
    private string NextSceneAddress;
    private bool isLoadScene = false;

    private AsyncOperationHandle sceneOperation;


    private void OnDestroy()
    {
        // Addressables.Release(sceneOperation);
    }

    private IEnumerator CoInitAddressables()
    {
        var initializeAsync = Addressables.InitializeAsync();
        yield return initializeAsync;

        var downloadOperation = Addressables.DownloadDependenciesAsync("default");

        while (!downloadOperation.IsDone)
        {
            Debug.Log(downloadOperation.PercentComplete * 100f);
            yield return null;
        }

        if (downloadOperation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
        {
            Debug.LogError("다운로드 실패");
            yield break;
        }

        Addressables.Release(downloadOperation);
        sceneOperation = Addressables.LoadSceneAsync(NextSceneAddress, LoadSceneMode.Single);

        while (!sceneOperation.IsDone)
        {
            Debug.Log(sceneOperation.PercentComplete * 100f);

            yield return null;

        }

        Addressables.Release(sceneOperation);
    }

    public void OnLoad()
    {
        if (!isLoadScene)
        {
            // Addressables.LoadSceneAsync(NextSceneAddress);

            StartCoroutine(CoInitAddressables());
            onStartLoadSceneEvent?.Invoke();
            isLoadScene = true;
        }
    }
}
