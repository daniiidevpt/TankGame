using UnityEngine;

namespace Core.SingletonSystem
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T m_Instance;
        private static bool m_IsQuitting;

        protected virtual bool IsPersistent => false;

        public static T Instance
        {
            get
            {
                if (m_IsQuitting)
                {
                    Debug.LogWarning($"[Singleton] Tried to access {typeof(T)} while quitting.");
                    return null;
                }

                if (m_Instance)
                    return m_Instance;

                m_Instance = FindAnyObjectByType<T>();

                if (!m_Instance)
                    Debug.LogWarning($"[Singleton] No instance found for type {typeof(T)}.");

                return m_Instance;
            }
        }

        public static bool TryGetInstance(out T instance)
        {
            instance = Instance;
            return instance != null;
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying)
                return;

            if (!m_Instance)
            {
                m_Instance = this as T;

                if (IsPersistent)
                    DontDestroyOnLoad(gameObject);

                OnInitialization();
            }
            else if (m_Instance != this)
            {
                Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T)} detected. Destroying {gameObject.name}.");
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            m_IsQuitting = true;
        }

        protected virtual void OnDestroy()
        {
            if (m_Instance == this)
            {
                OnDispose();
                m_Instance = null;
            }
        }

        protected virtual void OnInitialization() { }
        protected virtual void OnDispose() { }
    }
}

