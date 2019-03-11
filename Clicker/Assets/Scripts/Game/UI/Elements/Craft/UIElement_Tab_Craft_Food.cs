using clicker.datatables;
using UnityEngine;

namespace clicker.general.ui.windows
{
    public class UIElement_Tab_Craft_Food : UIElement_Tab_Craft_TabWithSlots<UIElement_FoodSlotsController>
    {
        public override void InitTab()
        {
            if (!m_IsInitialized)
            {
                Debug.Log("UIElement_Tab_Craft_Food: InitTab");

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

        protected override void AddItemToSlot(int index, DataTableItems.ItemTypes type)
        {
            //Добавить еду в ячейку выбранного оружия
            GameManager.Instance.Manager_Battle.SelectedFoodManager.AddItem(index, type);
        }

        protected override void AddSlotButton_PressHandler(RectTransform buttonTransform)
        {
            GameManager.Instance.Manager_Battle.SelectedFoodManager.CurAddSlot--;
            buttonTransform.GetComponent<UIElement_AddItemSlot>().UpdateProgress(GameManager.Instance.Manager_Battle.SelectedFoodManager.CurAddSlot,
                                                                                 GameManager.Instance.Manager_Battle.SelectedFoodManager.TotalAddSlot);

            if (GameManager.Instance.Manager_Battle.SelectedFoodManager.CurAddSlot <= 0)
            {
                GameManager.Instance.Manager_Battle.SelectedFoodManager.AddSlot();
                buttonTransform.gameObject.SetActive(false);
            }
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
        }

        protected override void UnscribeFromEvents()
        {
            base.UnscribeFromEvents();

            //Отписаться от событие добавления оружия
            GameManager.Instance.Manager_Battle.SelectedFoodManager.OnAddItem -= UpdateLocalSlotsControllerState;

            //Отписаться от событие добавления слота
            GameManager.Instance.Manager_Battle.SelectedFoodManager.OnAddSlot -= AddSlotToLocalSlotsController;
        }
    }
}
