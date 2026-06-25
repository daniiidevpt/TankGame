using Core.ModulesSystem;
using Core.ModulesSystem.CommonModules;
using Gameplay.Player.Data;
using Gameplay.Projectiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player.Modules
{
    [System.Serializable]
    public class PlayerShootingModule : ModuleBase
    {
        private PlayerShootingData m_Data;
        private InventoryModule m_Inventory;
        private PlayerController m_Player;

        public override void OnInitialize(ModuleController owner)
        {
            base.OnInitialize(owner);

            m_Data = owner.GetData<PlayerShootingData>();
            m_Inventory = owner.GetModule<InventoryModule>();
            m_Player = owner as PlayerController;

            if (m_Data == null)
                Debug.LogError($"[PlayerShootingModule] Missing {typeof(PlayerShootingData)} on {owner.name}");

            if (m_Inventory == null)
                Debug.LogError($"[PlayerShootingModule] Missing InventoryModule on {owner.name}");
        }

        public override void OnEnabled() => SetActionEnabled(true);
        public override void OnDisabled() => SetActionEnabled(false);
        public override void OnDispose() => SetActionEnabled(false);

        private void SetActionEnabled(bool enabled)
        {
            if (m_Data?.ShootAction == null)
                return;

            if (enabled) m_Data.ShootAction.action.Enable();
            else m_Data.ShootAction.action.Disable();
        }

        public override void OnUpdate()
        {
            if (m_Data == null || m_Inventory == null || m_Player == null)
                return;

            if (!m_Data.ShootAction.action.WasPressedThisFrame())
                return;

            if (!m_Inventory.TryConsumeProjectile(out var prefab))
            {
                Debug.Log("[PlayerShootingModule] No projectiles!");
                return;
            }

            Vector3 aimPoint = GetAimPoint(m_Player.TurretShootingPoint.position.y);
            var go = Object.Instantiate(prefab, m_Player.TurretShootingPoint.position, m_Player.TurretShootingPoint.rotation);

            if (go.TryGetComponent<ProjectileBase>(out var projectile))
                projectile.Launch(aimPoint);
            else
                Debug.LogError($"[PlayerShootingModule] Prefab {prefab.name} has no ProjectileBase component");

            Debug.Log($"[PlayerShootingModule] Fired {prefab.name} | Remaining: {m_Inventory.ActiveCount}");
        }

        private Vector3 GetAimPoint(float shootHeight)
        {
            if (Camera.main == null)
                return Transform.position + Transform.forward * 50f;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            var aimPlane = new Plane(Vector3.up, new Vector3(0f, shootHeight, 0f));
            if (aimPlane.Raycast(ray, out float enter))
                return ray.GetPoint(enter);

            return Transform.position + Transform.forward * 50f;
        }
    }
}
