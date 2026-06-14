using Core.ModulesSystem;
using Gameplay.Player.Data;
using UnityEngine;

namespace Gameplay.Player.Modules
{
    [System.Serializable]
    public class PlayerRotationModule : ModuleBase
    {
        private PlayerController m_Player;
        private PlayerRotationData m_Data;
        private Camera m_Camera;

        public override void OnInitialize(ModuleController owner)
        {
            base.OnInitialize(owner);

            m_Player = owner as PlayerController;

            m_Data = owner.GetData<PlayerRotationData>();
            m_Camera = Camera.main;

            if (m_Data == null)
                Debug.LogError($"Missing Data In Module {this} - Data: {typeof(PlayerRotationData)} - Owner: {owner.name}");
        }

        public override void OnEnabled() => SetActionEnabled(true);

        public override void OnDisabled() => SetActionEnabled(false);

        public override void OnDispose() => SetActionEnabled(false);

        private void SetActionEnabled(bool enabled)
        {
            if (enabled) m_Data.AimAction.action.Enable();
            else m_Data.AimAction.action.Disable();
        }

        public override void OnLateUpdate()
        {
            Vector3 hullUp = m_Player.transform.up;

            Vector2 screenPosition = m_Data.AimAction.action.ReadValue<Vector2>();
            Ray ray = m_Camera.ScreenPointToRay(screenPosition);
            Plane aimPlane = new(hullUp, m_Player.Turret.position);

            if (!aimPlane.Raycast(ray, out float distance))
                return;

            Vector3 direction = Vector3.ProjectOnPlane(ray.GetPoint(distance) - m_Player.Turret.position, hullUp);

            if (direction.sqrMagnitude < 0.0001f)
                return;

            Quaternion target = Quaternion.LookRotation(direction, hullUp);
            m_Player.Turret.rotation = Quaternion.RotateTowards(m_Player.Turret.rotation, target, m_Data.RotationSpeed * Time.deltaTime);
        }
    }
}
