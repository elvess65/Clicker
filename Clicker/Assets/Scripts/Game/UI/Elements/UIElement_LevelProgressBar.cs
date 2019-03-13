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

        public void Init()
        {
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
