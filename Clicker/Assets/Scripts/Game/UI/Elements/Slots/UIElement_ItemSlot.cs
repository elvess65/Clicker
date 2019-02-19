using clicker.datatables;
using FrameworkPackage.UI.Windows;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// UI слот предмета
    /// </summary>
    public class UIElement_ItemSlot : UIElement_ClickableItem
    {
        public System.Action<int> OnSlotPress;

        public Text Text_ItemName;
        public Text Text_Amount;
        public Image Image_Progress;

        public int Index { get; private set; }
        public DataTableItems.ItemTypes Type { get; private set; }

        public void Init(DataTableItems.ItemTypes type, int index, int amount, float progress)
        {
            Index = index;

            SetItem(type, amount, progress);

            base.Init();
        }


        /// <summary>
        /// Вывести информацию о предмете
        /// </summary>
        /// <param name="type">Тип предмета</param>
        /// <param name="amount">Количество предмета</param>
        /// <param name="progress">Прогресс предмета</param>
        public void SetItem(DataTableItems.ItemTypes type, int amount, float progress)
        {
            Text_ItemName.text = type.ToString();
            Type = type;

            SetAmount(amount);
            SetSlotProgress(progress);
        }

        /// <summary>
        /// Отобразить прогресс предмета
        /// </summary>
        /// <param name="progress">Прогресс</param>
        public void SetSlotProgress(float progress)
        {
            Image_Progress.fillAmount = progress;
        }

        /// <summary>
        /// Отобразить количество предмета
        /// </summary>
        /// <param name="amount">Количество предмета</param>
        public void SetAmount(int amount)
        {
            Text_Amount.text = amount != 0 ? amount.ToString() : string.Empty;
        }


        protected override void Button_PressHandler()
        {
            base.Button_PressHandler();

            OnSlotPress?.Invoke(Index);
        }
    }
}
