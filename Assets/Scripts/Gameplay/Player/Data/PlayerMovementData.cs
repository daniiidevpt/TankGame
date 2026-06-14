using Core.ModulesSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player.Data
{
    [CreateAssetMenu(fileName = "PlayerMovementStaticData", menuName = "StaticData/Player/MovementStaticData", order = 1)]
    public class PlayerMovementData : ModuleStaticData
    {
        public InputActionReference MoveAction;

        [Header("Drive")]
        public float MaxSpeed;

        [Header("Steering")]
        public float TurnSpeed;

        [Header("Gravity & Ground")]
        public float Gravity;
        public float AlignSpeed;
        public float GroundCheckDistance;
    }
}
