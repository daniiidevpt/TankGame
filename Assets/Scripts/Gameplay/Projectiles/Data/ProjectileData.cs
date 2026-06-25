using Core.ModulesSystem;
using UnityEngine;

namespace Gameplay.Projectiles.Data
{
    public abstract class ProjectileData : ModuleStaticData
    {
        [Header("Impact")]
        public float Damage;

        [Header("Collision")]
        public float HitRadius = 0.2f;
        public LayerMask HitLayers;

        [Header("Lifetime")]
        public float Lifetime = 5f;
    }
}
