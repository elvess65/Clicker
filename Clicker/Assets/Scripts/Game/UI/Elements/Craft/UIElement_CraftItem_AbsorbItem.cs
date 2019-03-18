using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// UI потребляемого ресурса
    /// </summary>
    public class UIElement_CraftItem_AbsorbItem : UIElement_CraftItem_RequireItem
    {
        public Text Text_CurAmountItem;

        public void UpdateForAmount(int itemsAmount)
        {
            ShowAmountText(Amount * itemsAmount);
            UpdateState(Amount * itemsAmount);
        }


        protected override void ShowAmountText(int amount)
        {
            base.ShowAmountText(amount);

            Text_CurAmountItem.text = string.Format("({0})", DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(Type));
        }
    }
}
