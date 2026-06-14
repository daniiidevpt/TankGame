using UnityEngine;

namespace Core.SingletonSystem
{
    public abstract class PersistentSingleton<T> : Singleton<T> where T : Component
    {
        protected override bool IsPersistent => true;
    }
}