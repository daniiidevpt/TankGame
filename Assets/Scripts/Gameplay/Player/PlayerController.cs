using Core.Interfaces;
using Core.ModulesSystem;
using Gameplay.Player.Modules;
using Gameplay.TurnSystem;
using UnityEngine;

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
