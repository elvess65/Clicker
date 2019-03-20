using clicker.datatables;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// Базовый класс для UI панелей слотов предметов
    /// </summary>
    public abstract class UIElement_ItemSlotsController : MonoBehaviour
    {
        public RectTransform SlotParent;
        public RectTransform AddSlotParent;
        public Image Image_Selection;

        public List<UIElement_ItemSlot> ItemSlots { get; private set; }

        private int m_SelectedSlotIndex = -1;
        private bool m_AllowClickable = false;
        protected int m_ItemBagSize = 0;

        public void Init(DataTableItems.ItemTypes[] selectedItemTypes, bool allowClickable)
        {
            m_AllowClickable = allowClickable;
            Image_Selection.enabled = false;
            ItemSlots = new List<UIElement_ItemSlot>();

            CacheItemBagSize(GetFilterType());

            //Создать слоты
            for (int i = 0; i < selectedItemTypes.Length; i++)
                CreateSlot(selectedItemTypes[i], i, allowClickable);
        }


        /// <summary>
        /// Обновить состояния предмета
        /// </summary>
        public void UpdateItemsState(DataTableItems.ItemTypes[] selectedItemTypes)
        {
            for (int i = 0; i < selectedItemTypes.Length; i++)
            {
                Debug.Log("Update item state");
                var amountAndProgressData = GetAmountAndProgressForItem(selectedItemTypes[i]);
                ItemSlots[i].SetItem(selectedItemTypes[i], amountAndProgressData.amount, amountAndProgressData.progress, m_ItemBagSize);
            }
        }

        /// <summary>
        /// Обновить прогресс указанного типа предмета
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="progress">Прогресс</param>
        public void UpdateItemProgress(DataTableItems.ItemTypes type, float progress)
        {
            for (int i = 0; i < ItemSlots.Count; i++)
            {
                if (ItemSlots[i].Type.Equals(type))
                {
                    ItemSlots[i].SetSlotProgress(progress);
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
            for (int i = 0; i < ItemSlots.Count; i++)
            {
                if (ItemSlots[i].Type.Equals(type))
                {
                    ItemSlots[i].SetAmount(amount);
                    break;
                }
            }
        }


        /// <summary>
        /// Выделить слот с предметом либо первый слот (в случае ошибки)
        /// </summary>
        /// <param name="index">Индекс слота</param>
        public void SelectItem(int index)
        {
            try
            {
                ShowSelection(ItemSlots[index].ItemRectTransform);
                m_SelectedSlotIndex = index;
            }
            catch
            { }
        }

        /// <summary>
        /// Добавить слот
        /// </summary>
        public void AddSlot(DataTableItems.ItemTypes type)
        {
            CreateSlot(type, ItemSlots.Count, m_AllowClickable);

            if (m_SelectedSlotIndex >= 0)
                StartCoroutine(WaitFrameToSelectItem(m_SelectedSlotIndex));
        }

        /// <summary>
        /// Кешировать размер сумки для последующего вывода
        /// </summary>
        public void CacheItemBagSize(DataTableItems.ItemFilterTypes itemFilter)
        {
            m_ItemBagSize = DataManager.Instance.PlayerAccount.Inventory.BagsState.GetBagSize(itemFilter);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(50);
            strBuilder.AppendFormat("Selected item slots: {0}", ItemSlots.Count);

            if (ItemSlots.Count > 0)
            {
                strBuilder.Append("\n");
                for (int i = 0; i < ItemSlots.Count; i++)
                    strBuilder.AppendFormat(" - Slot index: {0}, Slot type: {1}\n", ItemSlots[i].Index, ItemSlots[i].Type);
            }

            return strBuilder.ToString();
        }


        void CreateSlot(DataTableItems.ItemTypes type, int slotIndex, bool allowClickable)
        {
            var amountAndProgressData = GetAmountAndProgressForItem(type);

            UIElement_ItemSlot item = Instantiate(GameManager.Instance.Manager_UI.WindowsManager.UIElement_ItemSlotPrefab, SlotParent);
            item.Init(type, 
                      slotIndex, 
                      amountAndProgressData.amount, 
                      amountAndProgressData.progress,
                      m_ItemBagSize);

            item.OnSlotPress += Item_PressHandler;
            item.EnableButton(allowClickable);

            ItemSlots.Add(item);
        }

        void ShowSelection(RectTransform rTransform)
        {
            if (!Image_Selection.enabled)
                Image_Selection.enabled = true;

            Image_Selection.transform.position = rTransform.position;
        }

        IEnumerator WaitFrameToSelectItem(int index)
        {
            yield return null;
            ShowSelection(ItemSlots[index].ItemRectTransform);
        }


        protected abstract void Item_PressHandler(int index);

        protected abstract (int amount, float progress) GetAmountAndProgressForItem(DataTableItems.ItemTypes type);

        protected abstract DataTableItems.ItemFilterTypes GetFilterType();
    }
}
