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
                        item.Init(type, 
                                  DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(type),
                                  GameManager.Instance.CraftItemFactory.GetProgressForItem(type),
                                  itemData.AllowAutocraft,
                                  itemData.RequiredItems);

                        item.OnItemPress += Item_PressHanlder;
                        item.OnAutoCraftToggled += ItemToggle_TryChangeValueHandle;

                        //Добавить объект в словарь для последующего доступа
                        if (!m_Items.ContainsKey(type))
                            m_Items.Add(type, item);

                        //Если объект в очереди автодобывания
                        if (GameManager.Instance.AutoCraftItemsController.ItemIsProcessing(type))
                            item.Toggle_AutoCraft.SetValue(true);
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

        /// <summary>
        /// Изменение состояния автодобывания предмета
        /// </summary>
        /// <param name="type">Тип выбранного предмета</param>
        /// <param name="isToggled">true если предмет должен быть быть добавлен в автодобывания, false - если убран</param>
        protected virtual void ItemToggle_TryChangeValueHandle(DataTableItems.ItemTypes type, bool isToggled)
        {
            //Если необходимо добавить предмет в автодобычу
            if (isToggled)
            {
                //Если нельзя собрать предмет
                if (!DataManager.Instance.PlayerAccount.Inventory.CanCraftItem(type))
                {
                    Debug.LogError("CANT CRAFT ITEM");
                    PlayAutoCraftAnimation_Error(type);
                    return;
                }

                //Попытаться добавить предмет
                //Если нельзя 
                if (!GameManager.Instance.AutoCraftItemsController.AddItemToProcessing(type))
                {
                    Debug.LogError("CANNT PROCESS ITEM. MAX ITEMS PROCESSED");
                    PlayAutoCraftAnimation_Error(type);
                }
                else //Если можно
                {
                    //Изменить состояние Toggle
                    SetValueAutoCraftToggle(type, isToggled);
                }
            }
            else //Если необходимо убрать предмет из автодобычи
            {
                //Убрать предмет из автодобычи
                GameManager.Instance.AutoCraftItemsController.RemoveItemFromProcessing(type);

                //Изменить состояние Toggle
                SetValueAutoCraftToggle(type, isToggled);
                
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

            //Подписаться на событие изменения прогремма автодобывания
            GameManager.Instance.AutoCraftItemsController.OnProgress += AutoCraft_ProgressChanged_Handler;
            //Подписаться на событие  успешного окончания периода автодобывания
            GameManager.Instance.AutoCraftItemsController.OnPeriodFinished_Success += AutoCraft_PeriodFinished_Success_Handler;
            //Подписаться на событие  окончания периода автодобывания с ошибкой
            GameManager.Instance.AutoCraftItemsController.OnPeriodFinished_Error += AutoCraft_PeriodFinished__Error_Handler;
        }

        /// <summary>
        /// Отписаться от события 
        /// </summary>
        protected virtual void UnscribeFromEvents()
        {
            //Отписаться от события добавления тиков
            GameManager.Instance.CraftItemFactory.OnTickToItemAdded -= TickToItemAdded_Handler;
            
            //Отписаться от события создания предмета
            GameManager.Instance.CraftItemFactory.OnItemCrafted -= ItemCrafted_Handler;

            //Отписаться от события изменения прогремма автодобывания
            GameManager.Instance.AutoCraftItemsController.OnProgress -= AutoCraft_ProgressChanged_Handler;
            //Отписаться от события успешного окончания периода автодобывания
            GameManager.Instance.AutoCraftItemsController.OnPeriodFinished_Success -= AutoCraft_PeriodFinished_Success_Handler;
            //Отписаться от события окончания периода автодобывания с ошибкой
            GameManager.Instance.AutoCraftItemsController.OnPeriodFinished_Error -= AutoCraft_PeriodFinished__Error_Handler;
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


        void AutoCraft_ProgressChanged_Handler(DataTableItems.ItemTypes type, float progress)
        {
            SetAutoCraftTickProgress(type, progress);
        }

        void AutoCraft_PeriodFinished_Success_Handler(DataTableItems.ItemTypes type)
        {
            SetAutoCraftTickProgress(type, 0);
            PlayAutoCraftAnimation_Success(type);
        }

        void AutoCraft_PeriodFinished__Error_Handler(DataTableItems.ItemTypes type)
        {
            PlayAutoCraftAnimation_Error(type);
        }


        /// <summary>
        /// Задать прогресс тику автодобывания
        /// </summary>
        void SetAutoCraftTickProgress(DataTableItems.ItemTypes type, float progress)
        {
            if (m_Items.ContainsKey(type))
                m_Items[type].SetAutoCraftProgress(progress);
        }

        /// <summary>
        /// Проигрыать анимацию успеха автодобывания
        /// </summary>
        void PlayAutoCraftAnimation_Success(DataTableItems.ItemTypes type)
        {
            if (m_Items.ContainsKey(type))
                m_Items[type].Toggle_AutoCraft.PlaySuccessAnimation();
        }

        /// <summary>
        /// Проигрыать анимацию ошибки автодобывания
        /// </summary>
        void PlayAutoCraftAnimation_Error(DataTableItems.ItemTypes type)
        {
            if (m_Items.ContainsKey(type))
                m_Items[type].Toggle_AutoCraft.PlayErrorAnimation();
        }


        /// <summary>
        /// Изменить состояние Toggle автодобывания
        /// </summary>
        void SetValueAutoCraftToggle(DataTableItems.ItemTypes type, bool isToggled)
        {
            if (m_Items.ContainsKey(type))
                m_Items[type].Toggle_AutoCraft.SetValue(isToggled);
        }
    }
}
