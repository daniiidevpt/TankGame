using Core.ModulesSystem.Interfaces;
using UnityEngine;

namespace Core.ModulesSystem
{
    public abstract class ModuleBase : IModule
    {
        public bool IsEnabled { get; set; } = true;

        protected ModuleController Owner { get; private set; }

        protected Transform Transform => Owner.transform;

        public virtual void OnInitialize(ModuleController owner)
        {
            Owner = owner;
        }

        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnLateUpdate() { }

        public virtual void OnEnabled() { }
        public virtual void OnDisabled() { }

        public virtual void OnTriggerEnter(Collider other) { }
        public virtual void OnTriggerStay(Collider other) { }
        public virtual void OnTriggerExit(Collider other) { }

        public virtual void OnCollisionEnter(Collision collision) { }
        public virtual void OnCollisionStay(Collision collision) { }
        public virtual void OnCollisionExit(Collision collision) { }

        public virtual void OnDrawGizmos() { }

        public virtual void OnDispose() { }
    }
}
