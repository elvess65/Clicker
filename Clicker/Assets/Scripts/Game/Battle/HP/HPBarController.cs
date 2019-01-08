using UnityEngine;
using UnityEngine.UI;

namespace clicker.battle.HP
{
    public class HPBarController : MonoBehaviour
    {
        public Image Image_FG;
        public Text Text_Progress;

        private int m_CurHealth;
        private int m_Health;

        public void Init(int health)
        {
            m_CurHealth = m_Health = health;
        }

        public void UpdateHealth(int curHealth)
        {
            m_CurHealth = curHealth;
            Image_FG.fillAmount = m_CurHealth / (float)m_Health;
        }

        void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
