using FrameworkPackage.UI.Windows;
using UnityEngine;
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

        private int m_Index = 0;

        public void Init(datatables.DataTableItems.ItemTypes type, int index, int amount = 0)
        {
            m_Index = index;

            Text_ItemName.text = type.ToString();
            Text_Amount.text = amount != 0 ? amount.ToString() : string.Empty;

            base.Init();
        }

        protected override void Button_PressHandler()
        {
            base.Button_PressHandler();

            OnWeaponSlotPress?.Invoke(m_Index);
        }
    }
}
