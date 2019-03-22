using clicker.battle;
using clicker.datatables;
using UnityEngine;
using static clicker.account.Account.AccountUpgrades;

namespace clicker.general.ui.windows
{
    public class UIElement_Tab_Craft_Weapons : UIElement_Tab_Craft_TabWithSlots<UIElement_WeaponSlotsController, WeaponManager>
    {
        public override void InitTab()
        {
            if (!m_IsInitialized)
            {
                base.InitTab();

                InitializeSlots();
            }
            else
                UpdateTabState();

            SubscribeForEvents();
        }

   
        protected override UIElement_WeaponSlotsController GetSlotsController()
        {
            return GameManager.Instance.Manager_UI.CreateWeaponSlots(DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.ToArray(),
                                                                     SlotsParent,
                                                                     false,
                                                                     false);
        }

        protected override WeaponManager GetSelectedItemsManager()
        {
            return GameManager.Instance.Manager_Battle.SelectedWeaponManager;
        }

        protected override DataTableUpgrades.UpgradeTypes GetUpgradeType()
        {
            return DataTableUpgrades.UpgradeTypes.WeaponSlot;
        }

        protected override void AddItemToSlot(int index, DataTableItems.ItemTypes type)
        {
            //Добавить оружие в ячейку выбранного оружия
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.AddItem(index, type);
        }


        protected override void ItemCrafted_Handler(DataTableItems.ItemTypes craftedItemType)
        {
            base.ItemCrafted_Handler(craftedItemType);

            //Обновить UI количества оружия
            m_SlotsController.UpdateItemsState(DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.ToArray());

            //Обновить UI количества оружия
            GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemsState(DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.ToArray());
        }

        protected override void UpdateTabState()
        {
            base.UpdateTabState();

            m_SlotsController.UpdateItemsState(DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.ToArray());
        }

        protected override void SubscribeForEvents()
        {
            base.SubscribeForEvents();

            //Подписатья на событие добавления оружия
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.OnAddItem += UpdateLocalSlotsControllerState;

            //Подписатья на событие добавления слота
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.OnAddSlot += AddSlotToLocalSlotsController;
        }

        protected override void UnscribeFromEvents()
        {
            base.UnscribeFromEvents();

            //Отписаться от событие добавления оружия
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.OnAddItem -= UpdateLocalSlotsControllerState;

            //Отписаться от событие добавления слота
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.OnAddSlot -= AddSlotToLocalSlotsController;
        }
    }
}
