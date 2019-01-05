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
            { }
        }

        /// <summary>
        /// Обновить прогресс прочности указанного типа оружия
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="progress">Прочность</param>
        public void UpdateItemDurability(DataTableItems.ItemTypes type, float progress)
        {
            for (int i = 0; i < m_WeaponSlots.Count; i++)
            {
                if (m_WeaponSlots[i].Type.Equals(type))
                {
                    m_WeaponSlots[i].SetDurability(progress);
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
            for (int i = 0; i < m_WeaponSlots.Count; i++)
            {
                if (m_WeaponSlots[i].Type.Equals(type))
                {
                    m_WeaponSlots[i].SetAmount(amount);
                    break;
                }
            }
        }

        /// <summary>
        /// Заменить тип оружия в ячейке
        /// </summary>
        /// <param name="sourceType">Тип оружия, которое нужно заменить</param>
        /// <param name="targetType">Тип оружия на которое следует заменить</param>
        /// <param name="amount">Количество нового типа оружия</param>
        public void ReplaceWeapon(DataTableItems.ItemTypes sourceType, DataTableItems.ItemTypes targetType, int amount)
        {
            for (int i = 0; i < m_WeaponSlots.Count; i++)
            {
                if (m_WeaponSlots[i].Type.Equals(sourceType))
                {
                    m_WeaponSlots[i].SetWeapon(targetType, amount);
                    break;
                }
            }
        }


        void ShowSelection(RectTransform rTransform)
        {
            Image_Selection.transform.position = rTransform.position;
        }

        void Item_PressHandler(int index)
        {
            GameManager.Instance.Manager_Battle.SelectedWeaponManager.SelectWeapon(index);
        }
    }
}
