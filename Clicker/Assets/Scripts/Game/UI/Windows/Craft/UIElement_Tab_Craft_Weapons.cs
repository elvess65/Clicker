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
                    GameManager.Instance.Manager_UI.CreateWeaponSlots(DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.ToArray(),
                                                                      WeaponSlotsParent, 
                                                                      false, 
                                                                      false);

                //Создать кнопку добавления слотов оружия
                UIElement_AddItemSlot addSlotButton = GameManager.Instance.Manager_UI.CreateAddWeaponSlotButton(m_WeaponSlotsController.AddSlotParent);
                addSlotButton.OnItemClick += AddSlotButton_PressHandler;

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
            m_WeaponSlotsController.UpdateWeaponState(DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.ToArray());
        }

        protected override void UpdateTabState()
        {
            base.UpdateTabState();

            m_WeaponSlotsController.UpdateWeaponState(DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.ToArray());
        }

        protected override void SubscribeForEvents()
        {
            base.SubscribeForEvents();

            //Подписатья на событие добавления оружия
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.OnAddWeapon += UpdateWeaponState;

            //Подписатья на событие добавления слота
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.OnAddSlot += AddSlot_Handler;
        }

        protected override void UnscribeFromEvents()
        {
            base.UnscribeFromEvents();

            //Отписаться от событие добавления оружия
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.OnAddWeapon -= UpdateWeaponState;

            //Отписаться от событие добавления слота
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.OnAddSlot -= AddSlot_Handler;
        }


        /// <summary>
        /// Обновить состояния оружия
        /// </summary>
        void UpdateWeaponState(DataTableItems.ItemTypes[] selectedWeaponTypes)
        {
            m_WeaponSlotsController.UpdateWeaponState(selectedWeaponTypes);
        }


        /// <summary>
        /// Добавить слот
        /// </summary>
        void AddSlot_Handler(DataTableItems.ItemTypes type)
        {
            m_WeaponSlotsController.AddSlot(type);
        }

        void AddSlotButton_PressHandler(RectTransform buttonTransform)
        {
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.CurAddSlot--;
            buttonTransform.GetComponent<UIElement_AddItemSlot>().UpdateProgress(GameManager.Instance.Manager_Battle.SelectedWeaponManager.CurAddSlot, 
                                                                                   GameManager.Instance.Manager_Battle.SelectedWeaponManager.TotalAddSlot);

            if (GameManager.Instance.Manager_Battle.SelectedWeaponManager.CurAddSlot <= 0)
            {
                GameManager.Instance.Manager_Battle.SelectedWeaponManager.AddSlot();
                buttonTransform.gameObject.SetActive(false);
            }
        }


        void PointerDown_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
        }

        void Drag_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
        }

        void PointerUp_Handler(PointerEventData eventData, DataTableItems.ItemTypes type)
        {
            foreach (UIElement_ItemSlot item in m_WeaponSlotsController.ItemSlots)
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
