using Gameplay.TurnSystem.Interfaces;
using Core.ModulesSystem;
using Gameplay.Player.Modules;
using Gameplay.TurnSystem;
using UnityEngine;
using Core.ModulesSystem.CommonModules;

namespace Gameplay.Player
{
    public class PlayerController : ModuleController, ITurnParticipant
    {
        [SerializeField] private Transform m_Turret;
        [SerializeField] private Transform m_TurretShootingPoint;
        [SerializeField] private Transform m_CameraTarget;

        public Transform Turret => m_Turret;
        public Transform TurretShootingPoint => m_TurretShootingPoint;
        public Transform CameraTarget => m_CameraTarget;

        protected override void Awake()
        {
            AddModule<PlayerMovementModule>();
            AddModule<PlayerRotationModule>();
            AddModule<PlayerShootingModule>();

            //Common Modules
            AddModule<PickupModule>();
            AddModule<InventoryModule>();

            base.Awake();
        }

        protected override void Start()
        {
            TurnManager.Instance?.RegisterParticipant(this);
        }

        public void OnTurnBegin()
        {
            SetModuleEnabled<PlayerMovementModule>(true);
            SetModuleEnabled<PlayerRotationModule>(true);
            SetModuleEnabled<PlayerShootingModule>(true);
        }

        public void OnTurnEnd()
        {
            SetModuleEnabled<PlayerMovementModule>(false);
            SetModuleEnabled<PlayerRotationModule>(false);
            SetModuleEnabled<PlayerShootingModule>(false);
        }
    }
}
