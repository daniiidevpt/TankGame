using Core.ModulesSystem;
using Gameplay.Projectiles.Data;
using UnityEngine;

namespace Gameplay.Projectiles.Modules
{
    [System.Serializable]
    public class ArcFlightModule : ProjectileFlightModuleBase
    {
        private ArcFlightData m_Data;

        private Vector3 m_Start;
        private Vector3 m_Control;
        private Vector3 m_End;
        private Vector3 m_WobbleAxis;
        private float m_Duration;
        private float m_ArcFactor;
        private float m_FlightProgress;
        private bool m_Initialized;

        protected override ProjectileData Data => m_Data;

        public override void OnInitialize(ModuleController owner)
        {
            base.OnInitialize(owner);

            m_Data = owner.GetData<ArcFlightData>();

            if (m_Data == null)
                Debug.LogError($"[ArcFlightModule] Missing {typeof(ArcFlightData)} on {owner.name}");
        }

        protected override Vector3 ComputeNextPosition(float deltaTime)
        {
            if (!m_Initialized)
                InitFlight();

            m_FlightProgress = Mathf.Clamp01(m_FlightProgress + deltaTime / m_Duration);

            if (m_FlightProgress >= 1f)
            {
                m_FlightComplete = true;
                return m_End;
            }

            Vector3 arcPos = EvaluateBezier(m_FlightProgress);

            // Wobble fades as arc gets larger so it only shows on near-flat trajectories.
            float wobble = Mathf.Sin(m_FlightProgress * Mathf.PI * 2f * m_Data.WobbleFrequency)
                           * m_Data.WobbleAmplitude
                           * (1f - m_ArcFactor);

            return arcPos + m_WobbleAxis * wobble;
        }

        private void InitFlight()
        {
            m_Start = Projectile.SpawnPosition;
            m_End = Projectile.TargetPosition;

            float distance = Vector3.Distance(m_Start, m_End);
            m_ArcFactor = Mathf.Clamp01(distance / Mathf.Max(m_Data.ArcScaleDistance, 0.001f));

            float arcHeight = Mathf.Lerp(m_Data.MinArcHeight, m_Data.MaxArcHeight, m_ArcFactor);
            m_Control = (m_Start + m_End) * 0.5f + Vector3.up * arcHeight;

            m_Duration = Mathf.Max(distance / Mathf.Max(m_Data.Speed, 0.001f), 0.001f);

            Vector3 horizontal = m_End - m_Start;
            horizontal.y = 0f;
            m_WobbleAxis = horizontal.sqrMagnitude > 0.001f ? Vector3.Cross(Vector3.up, horizontal.normalized).normalized : Transform.right;

            m_FlightProgress = 0f;
            m_Initialized = true;
        }

        private Vector3 EvaluateBezier(float t)
        {
            float u = 1f - t;
            return u * u * m_Start
                 + 2f * u * t * m_Control
                 + t * t * m_End;
        }

        public override void OnDrawGizmos()
        {
            if (!m_Initialized || m_Data == null)
                return;

            Gizmos.color = Color.cyan;
            Vector3 prev = m_Start;
            for (int i = 1; i <= 20; i++)
            {
                Vector3 next = EvaluateBezier(i / 20f);
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
        }
    }
}
