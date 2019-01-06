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
                m_WeaponSlotsController = GameManager.Instance.Manager_UI.CreateWeaponSlots(GameManager.Instance.Manager_Battle.SelectedWeaponManager.SelectedWeapons.ToArray(),
                    WeaponSlotsParent, false, false);

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

            //Обновить локальный UI количества оружия 
            m_WeaponSlotsController.UpdateItemAmount(craftedItemType, GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(craftedItemType));
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

                    //Обновить UI окна
                    m_WeaponSlotsController.ReplaceWeaponInSlot(item.Index, type);

                    break;
                }
            }  
        }
    }
}
