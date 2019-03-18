using clicker.datatables;
using System.Collections.Generic;
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
        [Header("Absorbed Resources")]
        public RectTransform AbsorbedResourcesParent;

        private DataTableItems.ItemTypes m_ItemType;
        private Dictionary<DataTableItems.ItemTypes, UIElement_CraftItem_AbsorbItem> m_AbsorbingItems;

        public void Init(DataTableItems.ItemTypes itemType, float progress, float multiplayer)
        {
            m_ItemType = itemType;
            Text_ItemName.text = itemType.ToString();

            SetProgress(progress);
            SetMultiplayer(multiplayer);

            //Список потребляемых ресурсов
            ShowAbsorbedResources();
        }

        public void SetProgress(float progress)
        {
            Image_Progress.fillAmount = progress;
        }

        public void SetMultiplayer(float multiplayer)
        {
            Text_Multiplayer.text = string.Format("Abs:{0}%/sec", (int)(multiplayer * 100));
        }

        public void UpdateAbsorbedResourceItems()
        {
            foreach (DataTableItems.ItemTypes type in m_AbsorbingItems.Keys)
            {
                //Вывести количество предметов, которые необходимы единице популяции и умножить на количвество популяции
                m_AbsorbingItems[type].UpdateForAmount(DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(m_ItemType));
            }
        }


        void ShowAbsorbedResources()
        {
            m_AbsorbingItems = new Dictionary<DataTableItems.ItemTypes, UIElement_CraftItem_AbsorbItem>();

            DataTableItems.Item itemData = DataTableItems.GetItemDataByType(m_ItemType);
            for (int i = 0; i < itemData.RequiredItems.Length; i++)
            {
                //Создать предмет
                UIElement_CraftItem_AbsorbItem item = Instantiate(GameManager.Instance.Manager_UI.WindowsManager.UIElement_CraftAbsorbItemPrefab,
                                                                   AbsorbedResourcesParent);

                //Инициализировать как количество предметов, которые необходимы единице популяции 
                item.Init(itemData.RequiredItems[i].Type,
                          itemData.RequiredItems[i].Amount);

                //Вывести как значение для единицы популяции и умножить на количвество популяции
                item.UpdateForAmount(DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(m_ItemType));

                m_AbsorbingItems.Add(itemData.RequiredItems[i].Type, item);
            }
        }
    }
}
