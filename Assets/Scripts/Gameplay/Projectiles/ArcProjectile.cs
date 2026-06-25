using Gameplay.Projectiles.Modules;

namespace Gameplay.Projectiles
{
    public class ArcProjectile : ProjectileBase
    {
        protected override void Awake()
        {
            AddModule<ArcFlightModule>();
            base.Awake();
        }
    }
}
