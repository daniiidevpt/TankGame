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
        [SerializeField] private Transform m_CameraTarget;

        public Transform Turret => m_Turret;
        public Transform CameraTarget => m_CameraTarget;

        protected override void Awake()
        {
            AddModule<PlayerMovementModule>();
            AddModule<PlayerRotationModule>();

            //Common Modules
            AddModule<PickupModule>();

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
        }

        public void OnTurnEnd()
        {
            SetModuleEnabled<PlayerMovementModule>(false);
            SetModuleEnabled<PlayerRotationModule>(false);
        }
    }
}
