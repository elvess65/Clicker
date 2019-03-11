using clicker.datatables;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// UI текущего состояние поглощения ресурсов населением
    /// </summary>
    public class UIElement_PopulationProgressItem : MonoBehaviour
    {
        [Header("Texts")]
        public Text Text_ItemName;
        [Header("Images")]
        public Image Image_ItemIcon;
        public Image Image_Progress;

        private DataTableItems.ItemTypes m_ItemType;

        public void Init(DataTableItems.ItemTypes itemType, float progress)
        {
            m_ItemType = itemType;

            UpdateProgress(progress);
        }

        public void UpdateProgress(float progress)
        {
            Image_Progress.fillAmount = progress;
        }
    }
}
