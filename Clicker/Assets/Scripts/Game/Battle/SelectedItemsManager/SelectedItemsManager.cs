using clicker.datatables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace clicker.battle
{
    public abstract class SelectedItemsManager : MonoBehaviour
    {
        public System.Action<DataTableItems.ItemTypes[]> OnAddItem;
        public System.Action<DataTableItems.ItemTypes[]> OnRemoveItem;
        public System.Action<DataTableItems.ItemTypes> OnAddSlot;

        public int CurAddSlot = 5;
        public int TotalAddSlot = 5;

        protected int m_SelectedIndex = -1;

        public virtual void Init()
        {
            //Выделение с задержкой
            StartCoroutine(WaitFrameToSelectWeapon(0));
        }

        /// <summary>
        /// Выбрать предмет
        /// </summary>
        /// <param name="index">Индекс предмета из списка выбранного</param>
        public virtual void SelectItem(int index)
        {
            if (m_SelectedIndex == index)
                return;

            //Изменить индекс выделенного оружия
            m_SelectedIndex = index;
        }

        /// <summary>
        /// Использовать выбранный предмет
        /// </summary>
        public abstract int UseItem();

        /// <summary>
        /// Добавить предмет в слот
        /// </summary>
        /// <param name="index">Индекс слота</param>
        /// <param name="weaponType">Тип предмета</param>
        public void AddItem(int index, DataTableItems.ItemTypes weaponType)
        {
            try
            {
                //Если пытаемся добавить прредмет, а такое же оружие есть в другом слоте
                for (int i = 0; i < GetTargetItemList().Count; i++)
                {
                    if (i != index && GetTargetItemList()[i].Equals(weaponType))
                        GetTargetItemList()[i] = GetDefaultItem();
                }

                //Изменить список выбранных предметов
                GetTargetItemList()[index] = weaponType;

                //Вызвать событие изменения списка
                OnAddItem?.Invoke(GetTargetItemList().ToArray());
            }
            catch (System.Exception e)
            {
                Debug.LogError("Exeption: " + e.Message);
            }
        }

        /// <summary>
        /// Добавить слот для предмета
        /// </summary>
        public void AddSlot()
        {
            GetTargetItemList().Add(GetDefaultItem());

            OnAddSlot?.Invoke(GetDefaultItem());
        }


        protected virtual DataTableItems.ItemTypes GetDefaultItem()
        {
            return account.Account.AccountInventory.DEFAULT_ITEM;
        }

        protected abstract List<DataTableItems.ItemTypes> GetTargetItemList();


        protected DataTableItems.ItemTypes GetItemTypeByIndex(int index)
        {
            try
            {
                return GetTargetItemList()[index];
            }
            catch
            { }

            return GetDefaultItem();
        }

        IEnumerator WaitFrameToSelectWeapon(int index)
        {
            yield return null;
            SelectItem(index);
        }
    }
}
