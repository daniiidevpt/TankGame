using System.Collections.Generic;
using Core.ModulesSystem.Interfaces;
using UnityEngine;

namespace Core.ModulesSystem
{
    public class ModuleController : MonoBehaviour
    {
        [SerializeReference] protected List<IModule> m_Modules = new();
        [SerializeReference] protected List<ModuleStaticData> m_ModulesData = new();

        /// <summary>Creates and registers a new module of type T at runtime.</summary>
        public T AddModule<T>() where T : ModuleBase, new()
        {
            var module = new T();
            m_Modules.Add(module);
            return module;
        }

        /// <summary>Returns the first registered module of type T, or null.</summary>
        public T GetModule<T>() where T : class, IModule
        {
            foreach (var m in m_Modules)
                if (m is T match) return match;
            return null;
        }

        /// <summary>Removes and disposes the first registered module of type T.</summary>
        public bool RemoveModule<T>() where T : class, IModule
        {
            for (int i = 0; i < m_Modules.Count; i++)
            {
                if (m_Modules[i] is T module)
                {
                    module.OnDispose();
                    m_Modules.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>Enables or disables a module by type.</summary>
        public void SetModuleEnabled<T>(bool enabled) where T : class, IModule
        {
            var module = GetModule<T>();
            if (module == null) return;

            if (module.IsEnabled == enabled) return;

            module.IsEnabled = enabled;

            if (enabled) module.OnEnabled();
            else module.OnDisabled();
        }

        public T GetData<T>() where T : ModuleStaticData
        {
            foreach (var data in m_ModulesData)
                if (data is T match) return match;
            return null;
        }

        public T GetInterface<T>() where T : class
        {
            foreach (var module in m_Modules)
                if (module is T match) return match;
            return null;
        }

        protected virtual void Awake()
        {
            foreach (var module in m_Modules)
                module.OnInitialize(this);
        }

        protected virtual void Start() { }

        protected virtual void OnEnable()
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled) 
                    module.OnEnabled();
        }

        protected virtual void OnDisable()
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled) 
                    module.OnDisabled();
        }

        protected virtual void Update()
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled) 
                    module.OnUpdate();
        }

        protected virtual void FixedUpdate()
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled) 
                    module.OnFixedUpdate();
        }

        protected virtual void LateUpdate()
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled) 
                    module.OnLateUpdate();
        }

        protected virtual void OnDestroy()
        {
            foreach (var module in m_Modules)
                module.OnDispose();
            m_Modules.Clear();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled) 
                    module.OnTriggerEnter(other);
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled) 
                    module.OnTriggerStay(other);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled) 
                    module.OnTriggerExit(other);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled) 
                    module.OnCollisionEnter(collision);
        }

        protected virtual void OnCollisionStay(Collision collision)
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled) 
                    module.OnCollisionStay(collision);
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled) 
                    module.OnCollisionExit(collision);
        }

        private void OnDrawGizmos()
        {
            foreach (var module in m_Modules)
                if (module.IsEnabled)
                    module.OnDrawGizmos();
        }
    }
}