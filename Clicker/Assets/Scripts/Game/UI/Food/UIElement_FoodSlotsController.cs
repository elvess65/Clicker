using clicker.datatables;
using UnityEngine;

namespace clicker.general.ui
{
    /// <summary>
    /// UI панель слотов для еды
    /// </summary>
    public class UIElement_FoodSlotsController : UIElement_ItemSlotsController
    {
        protected override void Item_PressHandler(int index)
        {
            Debug.Log("Press at food slot at index : " + index);
        }

        protected override (int amount, float progress) GetAmountAndProgressForItem(DataTableItems.ItemTypes type)
        {
            return (5,
                    7);
        }
    }
}
