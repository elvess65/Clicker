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
            return GameManager.Instance.Manager_UI.CreateFoodSlots(GetSelectedFoodOrNone(),
                                                                   SlotsParent,
                                                                   false,
                                                                   false);
        }

        DataTableItems.ItemTypes[] GetSelectedFoodOrNone()
        {
            //TODO: Fill slots by selected food from inventory or ItemTypes.Max if slot is empty
            return new DataTableItems.ItemTypes[] { DataTableItems.ItemTypes.Max };
        }


        protected override void AddItemToSlot(int index, DataTableItems.ItemTypes type)
        {
            Debug.Log("ADD ITEM " + type + " TO SLOT");
        }

        protected override void AddSlotButton_PressHandler(RectTransform buttonTransform)
        {
            Debug.Log("ADD SLOT");
        }
    }
}
