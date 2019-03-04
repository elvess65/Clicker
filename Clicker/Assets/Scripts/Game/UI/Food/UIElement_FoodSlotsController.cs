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
            GameManager.Instance.Manager_Battle.SelectedFoodManager.SelectItem(index);
        }

        protected override (int amount, float progress) GetAmountAndProgressForItem(DataTableItems.ItemTypes type)
        {
            int amount = DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(type);
            return (amount, GetFoodProgress(amount));
        }

        float GetFoodProgress(int amount)
        {
            return (float)amount / DataManager.Instance.PlayerAccount.Inventory.FoodState.MaxFoodInSlot;
        }
    }
}
