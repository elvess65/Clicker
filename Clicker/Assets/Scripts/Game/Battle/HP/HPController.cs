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

        public HPController(int health, Transform hpBarParent)
        {
            m_CurrentHealth = m_MaxHealth = health;

            Debug.Log(GameManager.Instance);
            Debug.Log(GameManager.Instance.Manager_UI.WindowsManager);
            Debug.Log(GameManager.Instance.Manager_UI.WindowsManager.EnemyHPBarControllerPrefab);
            Debug.Log(hpBarParent);
            m_HPBarController = GameManager.Instantiate(GameManager.Instance.Manager_UI.WindowsManager.EnemyHPBarControllerPrefab, hpBarParent);
            m_HPBarController.Init(health);
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
