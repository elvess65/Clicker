using clicker.battle;
using clicker.datatables;
using UnityEngine;

namespace clicker.general.ui.windows
{
    public class UIElement_Tab_Craft_Food : UIElement_Tab_Craft_TabWithSlots<UIElement_FoodSlotsController, FoodManager>
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

        protected override UIElement_FoodSlotsController GetSlotsController()
        {
            return GameManager.Instance.Manager_UI.CreateFoodSlots(DataManager.Instance.PlayerAccount.Inventory.SelectedFood.ToArray(),
                                                                   SlotsParent,
                                                                   false,
                                                                   false);
        }

        protected override FoodManager GetSelectedItemsManager()
        {
            return GameManager.Instance.Manager_Battle.SelectedFoodManager;
        }

        protected override DataTableUpgrades.UpgradeTypes GetUpgradeType()
        {
            return DataTableUpgrades.UpgradeTypes.FoodSlot;
        }

        protected override void AddItemToSlot(int index, DataTableItems.ItemTypes type)
        {
            //Добавить еду в ячейку выбранного оружия
            GameManager.Instance.Manager_Battle.SelectedFoodManager.AddItem(index, type);
        }


        protected override void ItemCrafted_Handler(DataTableItems.ItemTypes craftedItemType)
        {
            base.ItemCrafted_Handler(craftedItemType);

            //Обновить UI количества оружия
            m_SlotsController.UpdateItemsState(DataManager.Instance.PlayerAccount.Inventory.SelectedFood.ToArray());

            //Обновить UI количества еды
            GameManager.Instance.Manager_UI.FoodSlotController.UpdateItemsState(DataManager.Instance.PlayerAccount.Inventory.SelectedFood.ToArray());
        }

        protected override void SubscribeForEvents()
        {
            base.SubscribeForEvents();

            //Подписатья на событие добавления оружия
            GameManager.Instance.Manager_Battle.SelectedFoodManager.OnAddItem += UpdateLocalSlotsControllerState;
            //Подписатья на событие добавления слота
            GameManager.Instance.Manager_Battle.SelectedFoodManager.OnAddSlot += AddSlotToLocalSlotsController;

            //Подписатся на событие изменения количества предметов периодом потребления населения
            GameManager.Instance.Manager_Battle.PopulationManager.OnPeriodChangedItemsAmount += PopulationPeriodChangedItemsAmountHandler;
        }

        protected override void UnscribeFromEvents()
        {
            base.UnscribeFromEvents();

            //Отписаться от события добавления оружия
            GameManager.Instance.Manager_Battle.SelectedFoodManager.OnAddItem -= UpdateLocalSlotsControllerState;
            //Отписаться от события добавления слота
            GameManager.Instance.Manager_Battle.SelectedFoodManager.OnAddSlot -= AddSlotToLocalSlotsController;

            //Отписатся от события изменения количества предметов периодом потребления населения
            GameManager.Instance.Manager_Battle.PopulationManager.OnPeriodChangedItemsAmount -= PopulationPeriodChangedItemsAmountHandler;
        }


        void PopulationPeriodChangedItemsAmountHandler()
        {
            UpdateTabState();
        }
    }
}
