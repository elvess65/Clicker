using UnityEngine;
using UnityEngine.UI;

namespace clicker.battle.HP
{
    public class HPBarController : MonoBehaviour
    {
        public Image Image_FG;
        public Text Text_Progress;
        public bool LookAtCamera = true;

        public void UpdateHealth(int curHealth, int health)
        {
            Image_FG.fillAmount = curHealth / (float)health;

            if (Text_Progress != null)
                Text_Progress.text = string.Format("{0}/{1}", curHealth, health);
        }

        void Update()
        {
            if (LookAtCamera)
                transform.LookAt(Camera.main.transform);
        }
    }
}
