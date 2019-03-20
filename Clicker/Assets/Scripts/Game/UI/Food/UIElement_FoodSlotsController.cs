using clicker.datatables;

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
            int amount = DataManager.Instance.PlayerAccount.Inventory.BagsState.GetItemAmountInBag(type);

            return (amount, GetFoodProgress(amount));
        }

        float GetFoodProgress(int amount)
        {
            return (float)amount / DataManager.Instance.PlayerAccount.Inventory.BagsState.GetBagSize(GetFilterType());
        }

        protected override DataTableItems.ItemFilterTypes GetFilterType()
        {
            return DataTableItems.ItemFilterTypes.Food;
        }
    }
}
