using Core.Interfaces;
using Core.ModulesSystem;
using UnityEngine;

namespace Core.ModulesSystem.CommonModules
{
    [System.Serializable]
    public class PickupModule : ModuleBase
    {
        public override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IPickable>(out var pickable))
                pickable.OnPickedUp(Owner);
        }
    }
}
