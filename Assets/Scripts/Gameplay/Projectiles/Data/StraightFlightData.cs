using UnityEngine;

namespace Gameplay.Projectiles.Data
{
    [CreateAssetMenu(fileName = "StraightFlightData", menuName = "StaticData/Projectiles/StraightFlightData")]
    public class StraightFlightData : ProjectileData
    {
        [Header("Flight")]
        public float Speed = 20f;
    }
}
