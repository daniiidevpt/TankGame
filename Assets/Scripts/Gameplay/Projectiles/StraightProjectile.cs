using Gameplay.Projectiles.Modules;

namespace Gameplay.Projectiles
{
    public class StraightProjectile : ProjectileBase
    {
        protected override void Awake()
        {
            AddModule<StraightFlightModule>();
            base.Awake();
        }
    }
}
