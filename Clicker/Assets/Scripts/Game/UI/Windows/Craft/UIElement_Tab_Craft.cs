using clicker.datatables;
using FrameworkPackage.UI.Windows;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui.windows
{
    /// <summary>
    /// Базовый класс для вкладок окна крафта
    /// </summary>
    public class UIElement_Tab_Craft : UITabContent
    {
        public DataTableItems.ItemFilterTypes FilterType;
        public RectTransform ItemsParent;
        public Text Text_Title;

        protected Dictionary<DataTableItems.ItemTypes, UIElement_CraftItem> m_Items;

        public override void InitTab()
        {
            if (!m_IsInitialized)
            {
                Text_Title.text = FilterType.ToString();

                m_Items = new Dictionary<DataTableItems.ItemTypes, UIElement_CraftItem>();

                //Создать предметы
                for (int i = 0; i < (int)DataTableItems.ItemTypes.Max; i++)
                {
                    //Получить тип предмета
                    DataTableItems.ItemTypes type = (DataTableItems.ItemTypes)i;

                    //Если предмет можно вывести в этой вкладке
                    if (DataTableItems.GetIemDataByType(type).MatchFilter(FilterType))
                    {
                        //Создать объект
                        UIElement_CraftItem item = Instantiate(GameManager.Instance.UIManager.WindowsManager.UIElement_CraftItemPrefab, ItemsParent);
                        item.Init(type, GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(type), GameManager.Instance.CraftItemFactory.GetProgressForItem(type));
                        item.OnItemPress += Item_PressHanlder;

                        //Добавить объект в словарь для последующего доступа
                        if (!m_Items.ContainsKey(type))
                            m_Items.Add(type, item);
                    }
                }

                //Подписаться на событие добавления тиков
                GameManager.Instance.CraftItemFactory.OnTickToItemAdded += TickToItemAdded_Handler;
                //Подписаться на событие создания предмета
                GameManager.Instance.CraftItemFactory.OnItemCrafted += ItemCrafted_Handler;

                base.InitTab();
            }
        }

        public override void DisposeOnWindowClose()
        {
            base.DisposeOnWindowClose();

            GameManager.Instance.CraftItemFactory.OnTickToItemAdded -= TickToItemAdded_Handler;
        }


        protected virtual void Item_PressHanlder(DataTableItems.ItemTypes type)
        {
            GameManager.Instance.CraftItemFactory.AddTickToItem(type);
        }

        protected virtual void TickToItemAdded_Handler(DataTableItems.ItemTypes type, float progress)
        {
            if (m_Items.ContainsKey(type))
                m_Items[type].UpdateProgress_AddTick(progress);
        }

        protected virtual void ItemCrafted_Handler(DataTableItems.ItemTypes type)
        {
            if (m_Items.ContainsKey(type))
                m_Items[type].UpdateProgress_Craft(GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(type));
        }
    }
}
