using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public UnityEvent onSceneSwitchEvent;

    public virtual void SwitchScene(string sceneName)
    {
        onSceneSwitchEvent?.Invoke();
        Time.timeScale = 1.0f;
        ObjectPoolManager.Instance.ObjectPoolTable.Clear();
        SceneLoader.Instance.SwitchScene(sceneName);
    }

    public virtual void SwitchDirectScene(string sceneName)
    {
        // SceneLoader.Instance.SwitchDirectScene(sceneName);
    }

    public virtual void SwitchScene(int SceneID)
    {
        SceneManager.LoadScene(SceneID);
    }

    public virtual void SwitchDirectScene(int SceneID)
    {
        Time.timeScale = 1.0f;
        ObjectPoolManager.Instance.ObjectPoolTable.Clear();
        SceneManager.LoadScene(SceneID);
    }
}
