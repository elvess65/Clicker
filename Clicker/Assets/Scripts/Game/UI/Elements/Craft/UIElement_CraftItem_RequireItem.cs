using clicker.datatables;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// UI необходимого для создания предмета объекта
    /// </summary>
    public class UIElement_CraftItem_RequireItem : MonoBehaviour
    {
        public Text Text_RequireItem;

        public DataTableItems.ItemTypes Type { get; private set; }
        public int Amount { get; private set; }

        public void Init(DataTableItems.ItemTypes type, int amount)
        {
            Type = type;
            Amount = amount;

            ShowAmountText(amount); 
            UpdateState();
        }

        public void UpdateState()
        {
            UpdateState(Amount);
        }


        protected void UpdateState(int amount)
        {
            bool accountHasItem = DataManager.Instance.PlayerAccount.Inventory.HasAmountOfItem(Type, amount);
            Text_RequireItem.color = accountHasItem ? Color.black : Color.red;
        }

        protected virtual void ShowAmountText(int amount)
        {
            Text_RequireItem.text = string.Format("{0}: {1}", Type, amount);
        }
    }
}
