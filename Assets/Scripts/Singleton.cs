using UnityEngine;

public class SingletonBehaviour <T>: MonoBehaviour where T: SingletonBehaviour<T>
{
    public static T Instance { get; protected set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
    }
}