using Core.ModulesSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player.Data
{
    [CreateAssetMenu(fileName = "PlayerRotationStaticData", menuName = "StaticData/Player/RotationStaticData", order = 2)]
    public class PlayerRotationData : ModuleStaticData
    {
        public InputActionReference AimAction;
        public float RotationSpeed;
    }
}
