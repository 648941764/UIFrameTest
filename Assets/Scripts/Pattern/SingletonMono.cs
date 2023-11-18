using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour, new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindAnyObjectByType<T>();
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake() 
    {
        if (instance != null)
        {
            DestroyImmediate(instance.gameObject);
            return;
        }
        instance = gameObject.GetComponent<T>();
        DontDestroyOnLoad(instance.gameObject);
        OnAwake(); 
    }
    protected virtual void OnAwake() { }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }
    protected virtual void OnDestroy() { }
}
