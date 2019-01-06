using clicker.datatables;
using FrameworkPackage.UI.Windows;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// UI слот для оружия
    /// </summary>
    public class UIElement_WeaponSlot : UIElement_ClickableItem
    {
        public System.Action<int> OnWeaponSlotPress;

        public Text Text_ItemName;
        public Text Text_Amount;
        public Image Image_DurabilityProgress;

        public int Index { get; private set; }
        public DataTableItems.ItemTypes Type { get; private set; }

        public void Init(DataTableItems.ItemTypes type, int index, int amount, float durabilityProgress)
        {
            Index = index;

            SetWeapon(type, amount, durabilityProgress);

            base.Init();
        }

        /// <summary>
        /// Отобразить прогресс прочности
        /// </summary>
        /// <param name="progress">Прогресс прочности</param>
        public void SetDurability(float progress)
        {
            Image_DurabilityProgress.fillAmount = progress;
        }

        /// <summary>
        /// Отобразить количество предмета
        /// </summary>
        /// <param name="amount">Количество предмета</param>
        public void SetAmount(int amount)
        {
            Text_Amount.text = amount != 0 ? amount.ToString() : string.Empty;
        }

        /// <summary>
        /// Вывести информацию об оружии
        /// </summary>
        /// <param name="type">Тип оружия</param>
        /// <param name="amount">Количество оружия</param>
        /// <param name="durabilityProgress">Текущее состояние оружия</param>
        public void SetWeapon(DataTableItems.ItemTypes type, int amount, float durabilityProgress)
        {
            Text_ItemName.text = type.ToString();
            Type = type;
            SetAmount(amount);
            SetDurability(durabilityProgress);
        }


        protected override void Button_PressHandler()
        {
            base.Button_PressHandler();

            OnWeaponSlotPress?.Invoke(Index);
        }
    }
}
