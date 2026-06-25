using Core.Interfaces;
using Core.ModulesSystem;
using Gameplay.Projectiles.Data;
using UnityEngine;

namespace Gameplay.Projectiles.Modules
{
    [System.Serializable]
    public abstract class ProjectileFlightModuleBase : ModuleBase
    {
        private float m_TimeAlive;
        protected bool m_FlightComplete;

        protected ProjectileBase Projectile { get; private set; }
        protected abstract ProjectileData Data { get; }

        public override void OnInitialize(ModuleController owner)
        {
            base.OnInitialize(owner);

            Projectile = owner as ProjectileBase;

            if (Projectile == null)
                Debug.LogError($"[{GetType().Name}] Owner must be a ProjectileBase on {owner.name}");
        }

        public sealed override void OnUpdate()
        {
            if (Data == null || Projectile == null || !Projectile.IsLaunched)
                return;

            m_TimeAlive += Time.deltaTime;
            if (m_TimeAlive >= Data.Lifetime)
                Object.Destroy(Owner.gameObject);
        }

        public sealed override void OnFixedUpdate()
        {
            if (Data == null || Projectile == null || !Projectile.IsLaunched)
                return;

            Vector3 nextPos = ComputeNextPosition(Time.fixedDeltaTime);

            Vector3 delta = nextPos - Transform.position;
            float dist = delta.magnitude;

            if (dist > 0.001f)
            {
                if (Physics.SphereCast(Transform.position, Data.HitRadius, delta / dist, out RaycastHit hit, dist, Data.HitLayers, QueryTriggerInteraction.Ignore))
                {
                    HandleImpact(hit);
                    return;
                }

                Transform.rotation = Quaternion.LookRotation(delta.normalized);
            }

            Transform.position = nextPos;
        }

        // Returns the desired position for the next frame.
        // Set m_FlightComplete = true to destroy on arrival.
        protected abstract Vector3 ComputeNextPosition(float deltaTime);

        private void HandleImpact(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(Data.Damage);

            Object.Destroy(Owner.gameObject);
        }
    }
}
