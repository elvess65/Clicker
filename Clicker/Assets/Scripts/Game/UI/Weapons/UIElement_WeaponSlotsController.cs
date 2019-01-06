using clicker.datatables;
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
        public RectTransform SlotPranet;
        public Image Image_Selection;

        public List<UIElement_WeaponSlot> WeaponSlots { get; private set; }

        public void Init(DataTableItems.ItemTypes[] selectedWeaponTypes, bool allowClickable)
        {
            Image_Selection.enabled = false;
            WeaponSlots = new List<UIElement_WeaponSlot>();

            for (int i = 0; i < selectedWeaponTypes.Length; i++)
            {
                var amountAndDurData = GetAmountAndDurabilityForWeapon(selectedWeaponTypes[i]);

                UIElement_WeaponSlot item = Instantiate(GameManager.Instance.Manager_UI.WindowsManager.UIElement_WeaponSlotPrefab, SlotPranet);
                item.Init(selectedWeaponTypes[i], i, amountAndDurData.amount, amountAndDurData.durabilityProgress);
                item.OnWeaponSlotPress += Item_PressHandler;
                item.EnableButton(allowClickable);

                WeaponSlots.Add(item);
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
            }
            catch
            { }
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
        /// Заменить тип оружия в ячейке определенного типа (Когда закончилось оружия и нужно заменить на другое)
        /// </summary>
        /// <param name="sourceType">Тип оружия, которое нужно заменить</param>
        /// <param name="targetType">Тип оружия на которое следует заменить</param>
        /// <param name="amount">Количество нового типа оружия</param>
        public void ReplaceWeapon(DataTableItems.ItemTypes sourceType, DataTableItems.ItemTypes targetType)
        {
            for (int i = 0; i < WeaponSlots.Count; i++)
            {
                if (WeaponSlots[i].Type.Equals(sourceType))
                {
                    var amountAndDurData = GetAmountAndDurabilityForWeapon(targetType);

                    WeaponSlots[i].SetWeapon(targetType, amountAndDurData.amount, amountAndDurData.durabilityProgress);
                    break;
                }
            }
        }

        /// <summary>
        /// Заменить тип оружия в конкретной ячейке (При выборе оружия)
        /// </summary>
        /// <param name="index">Индекс ячейки</param>
        /// <param name="type">Тип оружия</param>
        public void ReplaceWeaponInSlot(int index, DataTableItems.ItemTypes type)
        {
            var amountAndDurData = GetAmountAndDurabilityForWeapon(type);

            WeaponSlots[index].SetWeapon(type, amountAndDurData.amount, amountAndDurData.durabilityProgress);

            Debug.Log(ToString());
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
            return (GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(type),
                    GameManager.Instance.PlayerAccount.Inventory.WeaponState.GetDurabilityProgress(type));
        }
    }
}
