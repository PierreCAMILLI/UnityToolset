using UnityEngine;

/// <summary>
/// Provides a static persistant accessor to a Monobehaviour with only one instance
/// </summary>
public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    /// <summary>
    /// Static singleton instance of the Monobehaviour
    /// </summary>
    static T _instance = null;
    public static T Instance { get { return _instance; } }

    /// <summary>
    /// Indicates whether the Gameobject is marked as DontDestroyOnLoad
    /// </summary>
    [SerializeField]
    [Tooltip("Indicates whether the Gameobject is marked as DontDestroyOnLoad")]
    bool _dontDestroyOnLoad = false;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;

            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(transform.root.gameObject);

            if (!gameObject.name.Contains(" (Singleton)"))
                gameObject.name += " (Singleton)";
        }

        else if (_instance != this)
            Destroy(transform.root.gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
}