using UnityEngine;

namespace Core.ModulesSystem.Interfaces
{
    public interface IModule
    {
        bool IsEnabled { get; set; }

        void OnInitialize(ModuleController owner);

        void OnUpdate();
        void OnFixedUpdate();
        void OnLateUpdate();

        void OnEnabled();
        void OnDisabled();

        void OnTriggerEnter(Collider other);
        void OnTriggerStay(Collider other);
        void OnTriggerExit(Collider other);

        void OnCollisionEnter(Collision collision);
        void OnCollisionStay(Collision collision);
        void OnCollisionExit(Collision collision);

        void OnDrawGizmos();

        void OnDispose();
    }
}