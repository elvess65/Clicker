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
                    DataTableItems.Item itemData = DataTableItems.GetItemDataByType(type);
                    if (itemData != null && itemData.MatchFilter(FilterType))
                    {
                        //Создать объект
                        UIElement_CraftItem item = Instantiate(GameManager.Instance.Manager_UI.WindowsManager.UIElement_CraftItemPrefab, ItemsParent);
                        item.Init(type, DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(type), GameManager.Instance.CraftItemFactory.GetProgressForItem(type), itemData.RequiredItems);
                        item.OnItemPress += Item_PressHanlder;
                        item.OnAutoCraftToggled += ItemToggle_TryChangeValueHandle;

                        //Добавить объект в словарь для последующего доступа
                        if (!m_Items.ContainsKey(type))
                            m_Items.Add(type, item);
                    }
                }

                base.InitTab();
            }
        }

        public override void DisposeOnWindowClose()
        {
            base.DisposeOnWindowClose();

            UnscribeFromEvents();
        }

        public override void DeactivateTabOnSelectOther()
        {
            base.DeactivateTabOnSelectOther();

            UnscribeFromEvents();
        }


        /// <summary>
        /// Нажатие на UI предмета
        /// </summary>
        /// <param name="type">Тип предмета, на который было нажато</param>
        protected virtual void Item_PressHanlder(DataTableItems.ItemTypes type)
        {
            //Если можно - добавить тики определенному предметму
            if (DataManager.Instance.PlayerAccount.Inventory.CanCraftItem(type))
                GameManager.Instance.CraftItemFactory.AddTickToItem(type);
            else
                Debug.LogError("Cant craft item");
        }

        protected virtual void ItemToggle_TryChangeValueHandle(DataTableItems.ItemTypes type, bool isToggled, UIElement_Toggle sender)
        {
            //TODO
            //Remove sender
            //Replace sender with func

            //Если необходимо добавить предмет в автодобычу
            if (isToggled)
            {
                if (!DataManager.Instance.PlayerAccount.Inventory.CanCraftItem(type))
                {
                    Debug.LogError("CANT CRAFT ITEM");
                    sender.PlayErrorAnimation();
                    return;
                }

                //Попытаться добавить предмет
                //Если нельзя
                if (!GameManager.Instance.AutoCraftItemsController.AddItemToProcessing(type))
                {
                    Debug.LogError("CANNT PROCESS ITEM. MAX ITEMS PROCESSED");
                    sender.PlayErrorAnimation();
                }
                else //Если можно
                {
                    //Изменить состояние Toggle
                    sender.SetValue(isToggled);
                }
            }
            else //Если необходимо убрать предмет из автодобычи
            {
                //Убрать предмет из автодобычи
                GameManager.Instance.AutoCraftItemsController.RemoveItemFromProcessing(type);

                //Изменить состояние Toggle
                sender.SetValue(isToggled);
                
            }

            //В любом случае обнулить прогресс индикатора тика автодобычи
            SetAutoCraftTickProgress(type, 0);
        }

        /// <summary>
        /// Обработчик события добавления тиков предмету
        /// </summary>
        /// <param name="type">Тип предмета, которому были добавлены тики</param>
        /// <param name="progress">Текущий прогресс крафта</param>
        protected virtual void TickToItemAdded_Handler(DataTableItems.ItemTypes type, float progress)
        {
            //Обновить состояние предмета с учетом нового прогресса
            if (m_Items.ContainsKey(type))
                m_Items[type].UpdateProgress(progress);
        }

        /// <summary>
        /// Обработчик события крафта предмета
        /// </summary>
        /// <param name="craftedItemType">Тип предмета, который был скрафчен</param>
        protected virtual void ItemCrafted_Handler(DataTableItems.ItemTypes craftedItemType)
        {
            int craftedItemAmount = DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(craftedItemType);

            //Обновить состояние предмета 
            if (m_Items.ContainsKey(craftedItemType))
                m_Items[craftedItemType].UpdateAmount(craftedItemAmount);

            //Пройтись по всем UI объектам и обновить количество предметов, а так же состояние требуемых для создания предметов
            foreach (UIElement_CraftItem item in m_Items.Values)
            {
                //Не обновлять предмет, который был создан
                if (item.Type != craftedItemType)
                    item.SetItemAmount(DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(item.Type));

                //Состояние требуемых для создания предметов
                item.UpdateRequireItemsState();
            }
        }

        /// <summary>
        /// Подписаться на события
        /// </summary>
        protected virtual void SubscribeForEvents()
        {
            //Подписаться на событие добавления тиков
            GameManager.Instance.CraftItemFactory.OnTickToItemAdded += TickToItemAdded_Handler;

            //Подписаться на событие создания предмета
            GameManager.Instance.CraftItemFactory.OnItemCrafted += ItemCrafted_Handler;

            GameManager.Instance.AutoCraftItemsController.OnProgress += AutoCraftProgressChangedHandler;
            GameManager.Instance.AutoCraftItemsController.OnPeriodFinished_Success += AutoCraftPeriodFinishedHandler_Success;
            GameManager.Instance.AutoCraftItemsController.OnPeriodFinished_Error += AutoCraftPeriodFinishedHandler_Error;
        }

        /// <summary>
        /// Отписаться от события 
        /// </summary>
        protected virtual void UnscribeFromEvents()
        {
            //Отписаться на событие добавления тиков
            GameManager.Instance.CraftItemFactory.OnTickToItemAdded -= TickToItemAdded_Handler;
            
            //Отписаться на событие создания предмета
            GameManager.Instance.CraftItemFactory.OnItemCrafted -= ItemCrafted_Handler;

            GameManager.Instance.AutoCraftItemsController.OnProgress -= AutoCraftProgressChangedHandler;
            GameManager.Instance.AutoCraftItemsController.OnPeriodFinished_Success -= AutoCraftPeriodFinishedHandler_Success;
            GameManager.Instance.AutoCraftItemsController.OnPeriodFinished_Error -= AutoCraftPeriodFinishedHandler_Error;
        }


        /// <summary>
        /// Обновить состояние вкладки (обновить состояние UI объектов при повторном выделении вкладки)
        /// </summary>
        protected override void UpdateTabState()
        {
            foreach (UIElement_CraftItem item in m_Items.Values)
            {
                //Обновить количество предметов
                item.SetItemAmount(DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(item.Type));
                //Обновить прогресс предмета
                item.SetItemProgress(GameManager.Instance.CraftItemFactory.GetProgressForItem(item.Type));
                //Обновить состояние необходимых для создания предмета предметов
                item.UpdateRequireItemsState();
            }
        }


        void AutoCraftProgressChangedHandler(DataTableItems.ItemTypes type, float progress)
        {
            SetAutoCraftTickProgress(type, progress);
        }

        void AutoCraftPeriodFinishedHandler_Success(DataTableItems.ItemTypes type)
        {
            SetAutoCraftTickProgress(type, 0);
        }

        void AutoCraftPeriodFinishedHandler_Error(DataTableItems.ItemTypes type)
        {
            //TODO
            //Replace with func

            if (m_Items.ContainsKey(type))
                m_Items[type].Toggle_AutoCraft.PlayErrorAnimation();
        }

        void SetAutoCraftTickProgress(DataTableItems.ItemTypes type, float progress)
        {
            if (m_Items.ContainsKey(type))
                m_Items[type].SetAutoCraftProgress(progress);
        }
    }
}
