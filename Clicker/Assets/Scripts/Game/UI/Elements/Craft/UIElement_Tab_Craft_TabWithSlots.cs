using clicker.battle;
using clicker.datatables;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static clicker.account.Account.AccountUpgrades;

namespace clicker.general.ui.windows
{
    public abstract class UIElement_Tab_Craft_TabWithSlots<T, T1> : UIElement_Tab_Craft where T : UIElement_ItemSlotsController
                                                                                        where T1 : SelectedItemsManager
    {
        public RectTransform SlotsParent;

        protected T m_SlotsController;

        private UIElement_ItemDragIcon m_ItemDragIcon;

        protected void InitializeSlots()
        {
            //Создать панель оружия
            m_SlotsController = GetSlotsController();

            //Создать кнопку добавления слотов оружия если не достигнут последний уровень
            if (!DataTableUpgrades.IsLastLvl(GetUpgradeType(), DataManager.Instance.PlayerAccount.Upgrades.GetUpgradeLevel(GetUpgradeType())))
            {
                UIElement_AddItemSlot addSlotButton = GameManager.Instance.Manager_UI.CreateAddItemSlotButton(m_SlotsController.AddSlotParent, GetUpgradeType());
                addSlotButton.OnItemClick += AddSlotButton_PressHandler;
            }

            //Задать каждому предмету во вкладке события перетягивания
            foreach (UIElement_CraftItem item in m_Items.Values)
            {
                item.UIElement_DraggableIcon.enabled = true;

                item.OnPoinerDownEvent += PointerDown_Handler;
                item.OnDragEvent += Drag_Handler;
                item.OnPointerUpEvent += PointerUp_Handler;
            }
        }


        protected abstract T GetSlotsController();

        protected abstract T1 GetSelectedItemsManager();

        protected abstract DataTableUpgrades.UpgradeTypes GetUpgradeType();

        protected abstract void AddItemToSlot(int index, DataTableItems.ItemTypes type);


        /// <summary>
        /// Добавить слот
        /// </summary>
        protected void AddSlotToLocalSlotsController(DataTableItems.ItemTypes type)
        {
            m_SlotsController.AddSlot(type);
        }

        /// <summary>
        /// Обновить состояния предметов в локальном контроллере
        /// </summary>
        protected void UpdateLocalSlotsControllerState(DataTableItems.ItemTypes[] selectedItemTypes)
        {
            m_SlotsController.UpdateItemsState(selectedItemTypes);
        }

        /// <summary>
        /// Обработка нажатия на кнопку добавления слота
        /// </summary>
        /// <param name="senderTransform">Transform, на который нажали</param>
        protected void AddSlotButton_PressHandler(RectTransform senderTransform)
        {
            //Повысить уровень улучшения
            IncrementResult result = DataManager.Instance.PlayerAccount.Upgrades.IncrementUpgradeProgress(GetUpgradeType());
            switch (result)
            {
                //Если в результате повышения поднялся уровень - добавить слот и выключить кнопку
                case IncrementResult.IncrementLevel:
                    GetSelectedItemsManager().AddSlot();
                    senderTransform.gameObject.SetActive(false);
                    break;
                //Если в результате повышения повысился прогрес - обновить UI
                case IncrementResult.IncrementProgress:
                    senderTransform.GetComponent<UIElement_AddItemSlot>().UpdateProgress(GetUpgradeType());
                    break;
                //Если ничего не произошло - ничего не делать
                default:
                    break;
            }
        }


        void PointerDown_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
            if (m_ItemDragIcon == null)
            {
                m_ItemDragIcon = Instantiate(GameManager.Instance.AssetsLibrary.ItemDragIconPrefab, transform);
                m_ItemDragIcon.Init(type);
            }

            m_ItemDragIcon.transform.position = eventData.position;
        }

        void Drag_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
            m_ItemDragIcon.transform.position = eventData.position;
        }

        void PointerUp_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
            if (m_ItemDragIcon != null)
                Destroy(m_ItemDragIcon.gameObject);

            foreach (UIElement_ItemSlot item in m_SlotsController.ItemSlots)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(item.ItemRectTransform, Input.mousePosition))
                {
                    AddItemToSlot(item.Index, type);
                    break;
                }
            }
        }
    }
}
