using UnityEngine;

namespace Gameplay.Projectiles.Data
{
    [CreateAssetMenu(fileName = "ArcFlightData", menuName = "StaticData/Projectiles/ArcFlightData")]
    public class ArcFlightData : ProjectileData
    {
        [Header("Flight")]
        public float Speed = 15f;

        [Header("Arc")]
        public float MinArcHeight = 0.5f;
        public float MaxArcHeight = 8f;
        public float ArcScaleDistance = 30f;

        [Header("Wobble")]
        public float WobbleFrequency = 2f;
        public float WobbleAmplitude = 0.3f;
    }
}
