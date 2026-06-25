using Core.ModulesSystem;
using Gameplay.Projectiles.Data;
using UnityEngine;

namespace Gameplay.Projectiles.Modules
{
    [System.Serializable]
    public class StraightFlightModule : ProjectileFlightModuleBase
    {
        private StraightFlightData m_Data;
        private Vector3 m_Direction;
        private float m_MaxDistance;
        private float m_DistanceTraveled;
        private bool m_Initialized;

        protected override ProjectileData Data => m_Data;

        public override void OnInitialize(ModuleController owner)
        {
            base.OnInitialize(owner);

            m_Data = owner.GetData<StraightFlightData>();

            if (m_Data == null)
                Debug.LogError($"[StraightFlightModule] Missing {typeof(StraightFlightData)} on {owner.name}");
        }

        protected override Vector3 ComputeNextPosition(float deltaTime)
        {
            if (!m_Initialized)
            {
                Vector3 toTarget = Projectile.TargetPosition - Projectile.SpawnPosition;
                m_MaxDistance = toTarget.magnitude;
                m_Direction = m_MaxDistance > 0.001f ? toTarget / m_MaxDistance : Transform.forward;
                m_Initialized = true;
            }

            float step = m_Data.Speed * deltaTime;
            m_DistanceTraveled += step;

            if (m_DistanceTraveled >= m_MaxDistance)
                m_FlightComplete = true;

            return Transform.position + m_Direction * step;
        }
    }
}
