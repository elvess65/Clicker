using UnityEngine;
using UnityEngine.UI;

namespace clicker.battle.HP
{
    public class HPBarController : MonoBehaviour
    {
        public Image Image_FG;
        public Text Text_Progress;

        public void Init(int health)
        {
        }

        void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
