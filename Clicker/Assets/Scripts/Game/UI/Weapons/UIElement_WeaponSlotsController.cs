using clicker.datatables;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// UI панель слотов для оружия
    /// </summary>
    public class UIElement_WeaponSlotsController : MonoBehaviour
    {
        public RectTransform SlotParent;
        public RectTransform AddSlotParent;
        public Image Image_Selection;

        public List<UIElement_WeaponSlot> WeaponSlots { get; private set; }

        private int m_SelectedSlotIndex = -1;
        private bool m_AllowClickable = false;

        public void Init(DataTableItems.ItemTypes[] selectedWeaponTypes, bool allowClickable)
        {
            m_AllowClickable = allowClickable;
            Image_Selection.enabled = false;
            WeaponSlots = new List<UIElement_WeaponSlot>();

            //Создать слоты оружия
            for (int i = 0; i < selectedWeaponTypes.Length; i++)
                CreateWeaponSlot(selectedWeaponTypes[i], i, allowClickable); 
        }


        /// <summary>
        /// Обновить состояния оружия
        /// </summary>
        public void UpdateWeaponState(DataTableItems.ItemTypes[] selectedWeaponTypes)
        {
            for (int i = 0; i < selectedWeaponTypes.Length; i++)
            {
                var amountAndDurData = GetAmountAndDurabilityForWeapon(selectedWeaponTypes[i]);
                WeaponSlots[i].SetWeapon(selectedWeaponTypes[i], amountAndDurData.amount, amountAndDurData.durabilityProgress);
            }
        }

        /// <summary>
        /// Обновить прогресс прочности указанного типа оружия
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="progress">Прочность</param>
        public void UpdateItemDurability(DataTableItems.ItemTypes type, float progress)
        {
            for (int i = 0; i < WeaponSlots.Count; i++)
            {
                if (WeaponSlots[i].Type.Equals(type))
                {
                    WeaponSlots[i].SetDurability(progress);
                    break;
                }
            }
        }

        /// <summary>
        /// Обновить количество указаннонр предмета
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="amount">Количество</param>
        public void UpdateItemAmount(DataTableItems.ItemTypes type, int amount)
        {
            for (int i = 0; i < WeaponSlots.Count; i++)
            {
                if (WeaponSlots[i].Type.Equals(type))
                {
                    WeaponSlots[i].SetAmount(amount);
                    break;
                }
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
                ShowSelection(WeaponSlots[index].ItemRectTransform);
                m_SelectedSlotIndex = index;
            }
            catch
            { }
        }

        /// <summary>
        /// Добавить слот оружия
        /// </summary>
        public void AddSlot(DataTableItems.ItemTypes type)
        {
            CreateWeaponSlot(type, WeaponSlots.Count, m_AllowClickable);

            if (m_SelectedSlotIndex >= 0)
                StartCoroutine(WaitFrameToSelectWeapon(m_SelectedSlotIndex));
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(50);
            strBuilder.AppendFormat("Selected weapon slots: {0}", WeaponSlots.Count);

            if (WeaponSlots.Count > 0)
            {
                strBuilder.Append("\n");
                for (int i = 0; i < WeaponSlots.Count; i++)
                    strBuilder.AppendFormat(" - Slot index: {0}, Slot type: {1}\n", WeaponSlots[i].Index, WeaponSlots[i].Type);
            }

            return strBuilder.ToString();
        }


        void CreateWeaponSlot(DataTableItems.ItemTypes type, int slotIndex, bool allowClickable)
        {
            var amountAndDurData = GetAmountAndDurabilityForWeapon(type);

            UIElement_WeaponSlot item = Instantiate(GameManager.Instance.Manager_UI.WindowsManager.UIElement_WeaponSlotPrefab, SlotParent);
            item.Init(type, slotIndex, amountAndDurData.amount, amountAndDurData.durabilityProgress);
            item.OnWeaponSlotPress += Item_PressHandler;
            item.EnableButton(allowClickable);

            WeaponSlots.Add(item);
        }

        void ShowSelection(RectTransform rTransform)
        {
            if (!Image_Selection.enabled)
                Image_Selection.enabled = true;

            Image_Selection.transform.position = rTransform.position;
        }

        void Item_PressHandler(int index)
        {
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.SelectWeapon(index);
        }

        (int amount, float durabilityProgress) GetAmountAndDurabilityForWeapon(DataTableItems.ItemTypes type)
        {
            return (DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(type),
                    DataManager.Instance.PlayerAccount.Inventory.WeaponState.GetDurabilityProgress(type));
        }

        IEnumerator WaitFrameToSelectWeapon(int index)
        {
            yield return null;
            ShowSelection(WeaponSlots[index].ItemRectTransform);
        }
    }
}
