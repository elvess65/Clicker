using clicker.datatables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// UI панель слотов для оружия
    /// </summary>
    public class UIElement_WeaponSlotsController : MonoBehaviour
    {
        public RectTransform SlotPranet;
        public Image Image_Selection;

        private List<UIElement_WeaponSlot> m_WeaponSlots;

        public void Init(DataTableItems.ItemTypes[] selectedWeapons)
        {
            m_WeaponSlots = new List<UIElement_WeaponSlot>();

            for (int i = 0; i < selectedWeapons.Length; i++)
            {
                UIElement_WeaponSlot item = Instantiate(GameManager.Instance.Manager_UI.WindowsManager.UIElement_WeaponSlotPrefab, SlotPranet);
                item.Init(selectedWeapons[i], i, GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(selectedWeapons[i]));
                item.OnWeaponSlotPress += Item_PressHandler;

                m_WeaponSlots.Add(item);
            }
        }

        /// <summary>
        /// Выделить слот с оружием либо первый слот (в случае ошибки)
        /// </summary>
        /// <param name="index">Индекс слота</param>
        public void SelectItem(int index)
        {
            try
            {
                ShowSelection(m_WeaponSlots[index].ItemRectTransform);
            }
            catch
            {
                ShowSelection(m_WeaponSlots[0].ItemRectTransform);
            }
        }


        void ShowSelection(RectTransform rTransform)
        {
            Image_Selection.transform.position = rTransform.position;
        }

        void Item_PressHandler(int itemIndex)
        {
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.SelectWeapon(itemIndex);
        }
    }
}
