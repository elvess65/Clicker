using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    public class UIElement_LevelProgressBar : MonoBehaviour
    {
        [Header("Images")]
        public Image Image_Progress;
        public Image Image_Icon;
        [Header("Texts")]
        public Text Text_Progress;
        public Text Text_Level;

        public void Init(int level)
        {
            Text_Level.text = string.Format("Lvl. {0}", level + 1);

            SetProgress(0);
        }

        public void SetProgress(float progress)
        {
            int percentProgress = (int)(progress * 100);
            Text_Progress.text = percentProgress < 100 ? string.Format("{0}%", percentProgress) : "Complete";
            Image_Progress.fillAmount = progress;
        }
    }
}
