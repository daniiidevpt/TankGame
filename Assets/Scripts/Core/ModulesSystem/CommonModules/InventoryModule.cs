using System.Collections.Generic;
using UnityEngine;

namespace Core.ModulesSystem.CommonModules
{
    [System.Serializable]
    public class InventoryModule : ModuleBase
    {
        private readonly Dictionary<GameObject, int> m_Slots = new();
        private GameObject m_ActivePrefab;

        public bool HasProjectiles => m_ActivePrefab != null && m_Slots.TryGetValue(m_ActivePrefab, out int count) && count > 0;
        public int ActiveCount => m_ActivePrefab != null && m_Slots.TryGetValue(m_ActivePrefab, out int count) ? count : 0;
        public string ActiveName => m_ActivePrefab != null ? m_ActivePrefab.name : "None";

        public void AddProjectile(GameObject prefab, int amount = 1)
        {
            if (m_Slots.ContainsKey(prefab))
                m_Slots[prefab] += amount;
            else
            {
                m_Slots[prefab] = amount;
                if (m_ActivePrefab == null)
                    m_ActivePrefab = prefab;
            }

            Debug.Log($"[InventoryModule] +{amount} {prefab.name} | Slot: {m_Slots[prefab]}");
        }

        public bool TryConsumeProjectile(out GameObject prefab)
        {
            prefab = null;

            if (!HasProjectiles)
                return false;

            prefab = m_ActivePrefab;
            m_Slots[m_ActivePrefab]--;

            if (m_Slots[m_ActivePrefab] <= 0)
            {
                m_Slots.Remove(m_ActivePrefab);
                m_ActivePrefab = GetFirstKey();
            }

            return true;
        }

        public void CycleSlot(int direction = 1)
        {
            if (m_Slots.Count <= 1)
                return;

            var keys = new List<GameObject>(m_Slots.Keys);
            int index = keys.IndexOf(m_ActivePrefab);
            m_ActivePrefab = keys[(index + direction + keys.Count) % keys.Count];
        }

        private GameObject GetFirstKey()
        {
            foreach (var key in m_Slots.Keys)
                return key;
            return null;
        }
    }
}
