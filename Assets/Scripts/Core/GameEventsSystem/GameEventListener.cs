using UnityEngine;
using UnityEngine.Events;

namespace Core.GameEventsSystem
{
    public abstract class GameEventListener<T> : MonoBehaviour
    {
        [SerializeField] private GameEvent<T> m_Event;
        [SerializeField] private UnityEvent<T> m_Response;


        protected virtual void OnEnable()
        {
            if (m_Event != null)
                m_Event.Register(this, OnEventRaised);
        }

        protected virtual void OnDisable()
        {
            if (m_Event != null)
                m_Event.Unregister(this);
        }

        private void OnEventRaised(T value)
        {
            m_Response?.Invoke(value);
        }
    }
}
