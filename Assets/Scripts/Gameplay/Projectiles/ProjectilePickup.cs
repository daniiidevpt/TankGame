using Core.Interfaces;
using Core.ModulesSystem;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Projectiles
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class ProjectilePickup : MonoBehaviour, IPickable
    {
        private Rigidbody m_Rigidbody;
        private BoxCollider m_Collider;

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.isKinematic = true;

            m_Collider = GetComponent<BoxCollider>();
            m_Collider.isTrigger = true;
        }

        public void OnPickedUp(ModuleController collector)
        {
            if (collector is PlayerController playerController)
            {
                Debug.Log("Picked by player");
                Destroy(gameObject);
            }
        }
    }
}
