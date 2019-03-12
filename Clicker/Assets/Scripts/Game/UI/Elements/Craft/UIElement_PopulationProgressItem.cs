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
        public Text Text_Multiplayer;
        [Header("Images")]
        public Image Image_ItemIcon;
        public Image Image_Progress;
        public Image Image_Status;

        private DataTableItems.ItemTypes m_ItemType;

        public void Init(DataTableItems.ItemTypes itemType, float progress, float multiplayer)
        {
            m_ItemType = itemType;
            Text_ItemName.text = itemType.ToString();

            SetProgress(progress);
            SetMultiplayer(multiplayer);
            SetStatus(false);
        }

        public void SetProgress(float progress)
        {
            Image_Progress.fillAmount = progress;
        }

        public void SetMultiplayer(float multiplayer)
        {
            Text_Multiplayer.text = multiplayer.ToString();
        }

        public void SetStatus(bool isReducingPopulation)
        {
            Image_Status.color = isReducingPopulation ? Color.red : Color.green;
        }
    }
}
