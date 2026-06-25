using Core.ModulesSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player.Data
{
    [CreateAssetMenu(fileName = "PlayerShootingStaticData", menuName = "StaticData/Player/ShootingStaticData", order = 3)]
    public class PlayerShootingData : ModuleStaticData
    {
        [Header("Input")]
        public InputActionReference ShootAction;
    }
}
