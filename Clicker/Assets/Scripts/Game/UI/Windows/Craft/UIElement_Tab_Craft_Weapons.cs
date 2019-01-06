using clicker.datatables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace clicker.general.ui.windows
{
    public class UIElement_Tab_Craft_Weapons : UIElement_Tab_Craft
    {
        public RectTransform WeaponSlotsParent;

        private UIElement_WeaponSlotsController m_WeaponSlotsController;

        public override void InitTab()
        {
            if (!m_IsInitialized)
            {
                Debug.Log("UIElement_Tab_Craft_Weapons: InitTab");

                base.InitTab();

                //Создать панель оружия
                m_WeaponSlotsController = 
                    GameManager.Instance.Manager_UI.CreateWeaponSlots(GameManager.Instance.Manager_Battle.SelectedWeaponManager.SelectedWeapons.ToArray(),
                                                                      WeaponSlotsParent, 
                                                                      false, 
                                                                      false);

                //Создать кнопку добавления слотов оружия
                GameManager.Instance.Manager_UI.CreateAddWeaponSlotButton(m_WeaponSlotsController.AddSlotParent);

                //Задать каждому предмету во вкладке события перетягивания
                foreach (UIElement_CraftItem item in m_Items.Values)
                {
                    item.OnPoinerDownEvent += PointerDown_Handler;
                    item.OnDragEvent += Drag_Handler;
                    item.OnPointerUpEvent += PointerUp_Handler;
                }
            }
            else
                UpdateTabState();

            SubscribeForEvents();
        }

        protected override void ItemCrafted_Handler(DataTableItems.ItemTypes craftedItemType)
        {
            base.ItemCrafted_Handler(craftedItemType);

            //Обновить UI количества оружия
            m_WeaponSlotsController.UpdateWeaponState(GameManager.Instance.Manager_Battle.SelectedWeaponManager.SelectedWeapons.ToArray());
        }

        protected override void UpdateTabState()
        {
            base.UpdateTabState();

            m_WeaponSlotsController.UpdateWeaponState(GameManager.Instance.Manager_Battle.SelectedWeaponManager.SelectedWeapons.ToArray());
        }

        protected override void SubscribeForEvents()
        {
            base.SubscribeForEvents();

            //Подписатья на событие добавления оружия
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.OnAddWeapon += UpdateWeaponState;
        }

        protected override void UnscribeFromEvents()
        {
            base.UnscribeFromEvents();

            //Подписатья на событие добавления оружия
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.OnAddWeapon -= UpdateWeaponState;
        }


        /// <summary>
        /// Обновить состояния оружия
        /// </summary>
        void UpdateWeaponState(DataTableItems.ItemTypes[] selectedWeaponTypes)
        {
            m_WeaponSlotsController.UpdateWeaponState(selectedWeaponTypes);
        }

        void PointerDown_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
        }

        void Drag_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
        }

        void PointerUp_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
            foreach (UIElement_WeaponSlot item in m_WeaponSlotsController.WeaponSlots)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(item.ItemRectTransform, Input.mousePosition))
                {
                    //Добавить оружие в ячейку выбранного оружия
                    GameManager.Instance.Manager_Battle.SelectedWeaponManager.AddWeapon(item.Index, type);

                    break;
                }
            }  
        }
    }
}
