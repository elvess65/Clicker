using UnityEngine;
using UnityEngine.UI;

namespace clicker.battle.HP
{
    public class HPBarController : MonoBehaviour
    {
        public Image Image_FG;
        public Text Text_Progress;

        private int m_Health;

        public void Init(int health)
        {
            m_Health = health;
        }

        public void UpdateHealth(float progress, int curHealth, int health)
        {
            Image_FG.fillAmount = curHealth / (float)m_Health;
        }

        void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
