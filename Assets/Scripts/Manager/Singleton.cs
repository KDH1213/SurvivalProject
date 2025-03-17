using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    [SerializeField]
    private bool isDontDestroy = true;

    public static T Instance
    {
        get
        {
            if (ReferenceEquals(instance, null))
            {
                instance = FindObjectOfType<T>();
                if (ReferenceEquals(instance, null))
                {
                    var gameObject = new GameObject(nameof(T));
                    instance = gameObject.AddComponent<T>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (ReferenceEquals(instance, null))
        {
            instance = this as T;

            if (isDontDestroy)
                DontDestroyOnLoad(gameObject);
        }
        else if (!ReferenceEquals(instance.gameObject, gameObject))
        {
            Destroy(gameObject);
        }
    }
}