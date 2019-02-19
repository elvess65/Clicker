using clicker.datatables;

namespace clicker.general.ui
{
    /// <summary>
    /// UI панель слотов для оружия
    /// </summary>
    public class UIElement_WeaponSlotsController : UIElement_ItemSlotsController
    {
        protected override void Item_PressHandler(int index)
        {
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.SelectWeapon(index);
        }

        protected override (int amount, float progress) GetAmountAndProgressForItem(DataTableItems.ItemTypes type)
        {
            return (DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(type),
                    DataManager.Instance.PlayerAccount.Inventory.WeaponState.GetDurabilityProgress(type));
        }
    }
}
