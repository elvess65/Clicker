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
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.SelectItem(index);
        }

        protected override (int amount, float progress) GetAmountAndProgressForItem(DataTableItems.ItemTypes type)
        {
            UnityEngine.Debug.Log(DataManager.Instance.PlayerAccount.Inventory.BagsState.GetItemAmountInBag(type));
            UnityEngine.Debug.Log(DataManager.Instance.PlayerAccount.Inventory.WeaponState.GetDurabilityProgress(type));

            return (DataManager.Instance.PlayerAccount.Inventory.BagsState.GetItemAmountInBag(type),
                    DataManager.Instance.PlayerAccount.Inventory.WeaponState.GetDurabilityProgress(type));
        }

        protected override DataTableItems.ItemFilterTypes GetFilterType()
        {
            return DataTableItems.ItemFilterTypes.Weapons;
        }
    }
}
