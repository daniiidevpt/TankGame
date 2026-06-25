using Core.ModulesSystem;
using UnityEngine;

namespace Gameplay.Projectiles
{
    public class ProjectileBase : ModuleController
    {
        public Vector3 SpawnPosition { get; private set; }
        public Vector3 TargetPosition { get; private set; }
        public bool IsLaunched { get; private set; }

        public void Launch(Vector3 targetPosition)
        {
            SpawnPosition = transform.position;
            TargetPosition = targetPosition;
            IsLaunched = true;
        }
    }
}
