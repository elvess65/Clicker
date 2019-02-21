using clicker.datatables;
using clicker.general;
using System.Collections.Generic;
using UnityEngine;

namespace clicker.battle
{
    public class FoodManager : SelectedItemsManager
    {
        public override void Init()
        {
            //UI
            GameManager.Instance.Manager_UI.CreateFoodSlots(DataManager.Instance.PlayerAccount.Inventory.SelectedFood.ToArray(),
                                                            GameManager.Instance.Manager_UI.UIParent_MiddleRight,
                                                            true,
                                                            true);

            //Подписать UI на событие
            OnAddItem += GameManager.Instance.Manager_UI.FoodSlotController.UpdateItemsState;
            OnAddSlot += GameManager.Instance.Manager_UI.FoodSlotController.AddSlot;

            base.Init();
        }

        /// <summary>
        /// Выбрать еду
        /// </summary>
        /// <param name="index">Индекс еды из списка выбранного</param>
        public override void SelectItem(int index)
        {
            if (m_SelectedIndex == index)
                return;

            base.SelectItem(index);

            //Обновить UI - выделение слота
            GameManager.Instance.Manager_UI.FoodSlotController.SelectItem(m_SelectedIndex);

            Debug.Log("FoodManager: SELECT FOOD. Slot: " + m_SelectedIndex + "." + 
                                               " Type: " + GetItemTypeByIndex(m_SelectedIndex) + "." + 
                                               " Amount: " + DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(GetItemTypeByIndex(m_SelectedIndex)));
        }


        protected override List<DataTableItems.ItemTypes> GetTargetItemList()
        {
            return DataManager.Instance.PlayerAccount.Inventory.SelectedFood;
        }

        protected override DataTableItems.ItemTypes GetDefaultItem()
        {
            return DataTableItems.ItemTypes.Max;
        }
    }
}
