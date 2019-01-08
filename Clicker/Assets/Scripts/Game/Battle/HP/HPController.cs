using clicker.general;
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
        private HPBarController m_HPBarController;

        public float CurrentProgress => Mathf.Clamp(m_CurrentHealth / m_MaxHealth, 0, 1);

        public HPController(int health, HPBarController hpBarPrefab, Transform hpBarParent)
        {
            //Инициализировать ХП
            m_CurrentHealth = m_MaxHealth = health;

            //Создание ХП бара
            m_HPBarController = GameManager.Instantiate(hpBarPrefab, hpBarParent);
            m_HPBarController.Init(health);
        }

        public void TakeDamage(int damage)
        {
            //Снятие ХП
            m_CurrentHealth -= damage;

            //Обновление ХП бара
            //if (m_HPBarController != null)
            //    m_HPBarController.UpdateHealth(m_CurrentHealth);

            //Уничтожение
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
