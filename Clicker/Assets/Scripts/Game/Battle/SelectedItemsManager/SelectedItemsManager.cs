using clicker.datatables;
using clicker.general;
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

        protected int m_SelectedIndex = -1;

        public virtual void Init()
        {
            SubcribeForGlobalEvents();
            SubscribeForLocalEvents();

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
        /// <param name="itemType">Тип предмета</param>
        public void AddItem(int index, DataTableItems.ItemTypes itemType)
        {
            try
            {
                //Если пытаемся добавить предмет, а такой же есть в другом слоте - убираем его с занятого слота
                for (int i = 0; i < GetTargetItemList().Count; i++)
                {
                    if (i != index && GetTargetItemList()[i].Equals(itemType))
                    {
                        GetTargetItemList()[i] = GetDefaultItem();
                        DataManager.Instance.PlayerAccount.Inventory.BagsState.RemoveItemFromBag(GetTargetItemList()[i], true);
                    }
                }

                //Изменить список выбранных предметов
                GetTargetItemList()[index] = itemType;

                //Добавить предмет в сумку
                DataManager.Instance.PlayerAccount.Inventory.BagsState.AddItemToBag(itemType);

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

        public virtual void UnscribeFromGlobalEvents()
        {
        }


        protected virtual void SubcribeForGlobalEvents()
        {
        }

        protected virtual void SubscribeForLocalEvents()
        {
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
