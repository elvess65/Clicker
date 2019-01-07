using UnityEngine;

namespace clicker.battle.HP
{
    /// <summary>
    /// Класс HP 
    /// </summary>
    public class HPController 
    {
        public System.Action OnDestroyObject;

        private int m_CurrentHealth;
        private int m_MaxHealth;

        public float CurrentProgress => Mathf.Clamp(m_CurrentHealth / m_MaxHealth, 0, 1);

        public HPController(int health)
        {
            m_CurrentHealth = m_MaxHealth = health;
        }

        public void TakeDamage(int damage)
        {
            m_CurrentHealth -= damage;

            if (m_CurrentHealth < 0)
            {
                m_CurrentHealth = 0;

                DestroyObject();
            }
        }

        void DestroyObject()
        {
            OnDestroyObject?.Invoke();
        }
    }
}
