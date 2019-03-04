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

        public float CurrentProgress => Mathf.Clamp((float)m_CurrentHealth / m_MaxHealth, 0, 1);

        public HPController(int health, HPBarController hpBarPrefab, Transform hpBarParent)
        {
            //Инициализировать ХП
            m_CurrentHealth = m_MaxHealth = health;

            //Создание ХП бара
            m_HPBarController = GameManager.Instantiate(hpBarPrefab, hpBarParent);
            m_HPBarController.UpdateHealth(health, health);
        }

        public void TakeDamage(int damage)
        {
            //Снятие ХП
            m_CurrentHealth = Mathf.Clamp(m_CurrentHealth - damage, 0, m_CurrentHealth);

            //Обновление ХП бара
            if (m_HPBarController != null)
                m_HPBarController.UpdateHealth(m_CurrentHealth, m_MaxHealth);

            //Уничтожение 
            if (m_CurrentHealth == 0)
                DestroyObject();
        }

        public void AddHP(int amount)
        {
            //Добавление ХП
            m_CurrentHealth = Mathf.Clamp(m_CurrentHealth + amount, 0, m_MaxHealth);

            //Обновление ХП бара
            if (m_HPBarController != null)
                m_HPBarController.UpdateHealth(m_CurrentHealth, m_MaxHealth);
        }

        public void ResetHPBarPosition()
        {
            m_HPBarController.transform.localPosition = Vector3.zero;
        }


        void DestroyObject()
        {
            OnDestroyObject?.Invoke();
        }
    }
}
