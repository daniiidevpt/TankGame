using Core.Interfaces;
using UnityEngine;

namespace Core.ModulesSystem
{
    [System.Serializable]
    public class HealthModule : ModuleBase, IDamageable
    {
        [SerializeField] private float m_MaxHealth = 100f;
        [SerializeField] private float m_CurrentHealth;

        public bool IsDead { get; private set; }
        public float CurrentHealth => m_CurrentHealth;
        public float MaxHealth => m_MaxHealth;

        public override void OnInitialize(ModuleController owner)
        {
            base.OnInitialize(owner);
            m_CurrentHealth = m_MaxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) 
                return;

            m_CurrentHealth = Mathf.Max(0f, m_CurrentHealth - amount);

            if (m_CurrentHealth == 0f)
                Die();
        }

        public void Heal(float amount)
        {
            if (IsDead) 
                return;

            m_CurrentHealth = Mathf.Min(m_MaxHealth, m_CurrentHealth + amount);
        }

        private void Die()
        {
            IsDead = true;
            Owner.gameObject.SetActive(false);
        }
    }
}