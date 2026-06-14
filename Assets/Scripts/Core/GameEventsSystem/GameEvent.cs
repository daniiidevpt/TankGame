using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameEventsSystem
{
    public abstract class GameEvent<T> : ScriptableObject
    {
        [SerializeField] private bool m_Debug;

        private readonly List<(WeakReference owner, Action<T> callback)> m_Listeners = new();

        public void Raise(T value)
        {
            for (int i = m_Listeners.Count - 1; i >= 0; i--)
            {
                var (ownerRef, callback) = m_Listeners[i];
                var owner = ownerRef.Target;

                if (owner == null || (owner is UnityEngine.Object uo && uo == null))
                {
                    m_Listeners.RemoveAt(i);
                    continue;
                }

                try
                {
                    callback?.Invoke(value);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[GameEvent<{typeof(T).Name}>: {name}] Listener exception:\n{e}");
                }
            }

            if (m_Debug)
                Debug.Log($"<color=#8AE234>[GameEvent<{typeof(T).Name}]</color> <b>{name}</b> raised ({m_Listeners.Count} active listeners)");
        }

        public void Register(object owner, Action<T> callback)
        {
            if (owner == null || callback == null)
            {
                Debug.LogWarning($"[GameEvent<{typeof(T).Name}>: {name}] Invalid register attempt (null owner or callback)");
                return;
            }

            for (int i = 0; i < m_Listeners.Count; i++)
            {
                if (ReferenceEquals(m_Listeners[i].owner.Target, owner))
                {
                    m_Listeners[i] = (new WeakReference(owner), callback);

                    if (m_Debug)
                        Debug.Log($"[GameEvent<{typeof(T).Name}>: {name}] Replaced listener from {owner}");

                    return;
                }
            }

            m_Listeners.Add((new WeakReference(owner), callback));
        }

        public void Unregister(object owner)
        {
            if (owner == null) 
                return;

            for (int i = m_Listeners.Count - 1; i >= 0; i--)
            {
                var target = m_Listeners[i].owner.Target;
                if (ReferenceEquals(target, owner) ||
                    (target is UnityEngine.Object uo && uo == null))
                {
                    m_Listeners.RemoveAt(i);
                }
            }
        }

        public IReadOnlyList<(WeakReference owner, Action<T> callback)> GetListeners() => m_Listeners.AsReadOnly();
    }
}
