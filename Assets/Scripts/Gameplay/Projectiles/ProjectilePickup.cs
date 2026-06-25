using Core.Interfaces;
using Core.ModulesSystem;
using Core.ModulesSystem.CommonModules;
using UnityEngine;

namespace Gameplay.Projectiles
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class ProjectilePickup : MonoBehaviour, IPickable
    {
        private const float FLOAT_RANGE = 0.1f;

        [SerializeField] private GameObject m_ProjectilePrefab;
        [SerializeField] private float m_FloatSpeed = 1f;
        [SerializeField] private float m_RotationSpeed = 90f;

        private Rigidbody m_Rigidbody;
        private BoxCollider m_Collider;
        private Vector3 m_InitialPosition;

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.isKinematic = true;

            m_Collider = GetComponent<BoxCollider>();
            m_Collider.isTrigger = true;

            m_InitialPosition = transform.position;
        }

        private void Update()
        {
            float offset = Mathf.Sin(Time.time * m_FloatSpeed) * FLOAT_RANGE;
            transform.position = m_InitialPosition + Vector3.up * offset;

            transform.Rotate(Vector3.up, m_RotationSpeed * Time.deltaTime);
        }

        public void OnPickedUp(ModuleController collector)
        {
            if (m_ProjectilePrefab == null)
            {
                Debug.LogError($"[ProjectilePickup] No prefab assigned on {name}");
                return;
            }

            var inventory = collector.GetModule<InventoryModule>();
            if (inventory == null)
                return;

            inventory.AddProjectile(m_ProjectilePrefab);
            Destroy(gameObject);
        }
    }
}
