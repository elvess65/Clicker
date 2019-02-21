using clicker.datatables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace clicker.general.ui.windows
{
    public abstract class UIElement_Tab_Craft_TabWithSlots<T> : UIElement_Tab_Craft where T : UIElement_ItemSlotsController
    {
        public RectTransform SlotsParent;

        protected T m_SlotsController;

        protected void InitializeSlots()
        {
            //Создать панель оружия
            m_SlotsController = GetSlotsController();

            //Создать кнопку добавления слотов оружия
            UIElement_AddItemSlot addSlotButton = GameManager.Instance.Manager_UI.CreateAddItemSlotButton(m_SlotsController.AddSlotParent);
            addSlotButton.OnItemClick += AddSlotButton_PressHandler;

            //Задать каждому предмету во вкладке события перетягивания
            foreach (UIElement_CraftItem item in m_Items.Values)
            {
                item.OnPoinerDownEvent += PointerDown_Handler;
                item.OnDragEvent += Drag_Handler;
                item.OnPointerUpEvent += PointerUp_Handler;
            }
        }


        protected abstract T GetSlotsController();

        protected abstract void AddSlotButton_PressHandler(RectTransform buttonTransform);

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


        void PointerDown_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
        }

        void Drag_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
        }

        void PointerUp_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
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
