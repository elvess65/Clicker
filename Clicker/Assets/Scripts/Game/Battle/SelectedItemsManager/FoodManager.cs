using clicker.datatables;
using clicker.general;
using System.Collections.Generic;
using UnityEngine;

namespace clicker.battle
{
    public class FoodManager : SelectedItemsManager
    {
        private float m_LastClickTime = 0;

        private const float m_DOUBLE_CLICK_TIME = 1;

        public override void Init()
        {
            //UI
            GameManager.Instance.Manager_UI.CreateFoodSlots(DataManager.Instance.PlayerAccount.Inventory.SelectedFood.ToArray(),
                                                            GameManager.Instance.Manager_UI.UIParent_MiddleRight,
                                                            true,
                                                            true);

            //Подписать UI на событие
            OnAddItem += GameManager.Instance.Manager_UI.FoodSlotController.UpdateItemsState;
            OnRemoveItem += GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemsState;
            OnAddSlot += GameManager.Instance.Manager_UI.FoodSlotController.AddSlot;

            base.Init();
        }

        /// <summary>
        /// Выбрать еду
        /// </summary>
        /// <param name="index">Индекс еды из списка выбранного</param>
        public override void SelectItem(int index)
        {
            float timeBtwClick = Time.time - m_LastClickTime;
            m_LastClickTime = Time.time;
            if (timeBtwClick <= m_DOUBLE_CLICK_TIME)
            {
                if (GameManager.Instance.Manager_Battle.Player.HPController.CurrentProgress < 1)
                    GameManager.Instance.Manager_Battle.Player.HPController.AddHP(UseItem());

                m_LastClickTime = 0;
            }


            Debug.Log(m_SelectedIndex == index);
            if (m_SelectedIndex == index)
                return;

            base.SelectItem(index);

            //Обновить UI - выделение слота
            GameManager.Instance.Manager_UI.FoodSlotController.SelectItem(m_SelectedIndex);

            Debug.Log("FoodManager: SELECT FOOD. Slot: " + m_SelectedIndex + "." + 
                                               " Type: " + GetItemTypeByIndex(m_SelectedIndex) + "." + 
                                               " Amount: " + DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(GetItemTypeByIndex(m_SelectedIndex)));
        }

        public override int UseItem()
        {
            //Получить тип предмета
            DataTableItems.ItemTypes selectedFoodType = GetItemTypeByIndex(m_SelectedIndex);

            if (selectedFoodType != DataTableItems.ItemTypes.Max)
            {
                Debug.Log("USE ITEM " + selectedFoodType);

                //Удалить использованный предмет
                DataManager.Instance.PlayerAccount.Inventory.RemoveItem(selectedFoodType);

                //Если после использования предмета еще остались предметы этого типа
                if (!DataManager.Instance.PlayerAccount.Inventory.HasItem(selectedFoodType))
                {
                    //Найти слот с предметом, который использовался
                    for (int i = 0; i < DataManager.Instance.PlayerAccount.Inventory.SelectedFood.Count; i++)
                    {
                        //Удалить предмет из слота
                        if (DataManager.Instance.PlayerAccount.Inventory.SelectedFood[i].Equals(selectedFoodType))
                        {
                            DataManager.Instance.PlayerAccount.Inventory.SelectedFood[i] = DataTableItems.ItemTypes.Max;

                            //Найти UI слот и обновить состояние
                            GameManager.Instance.Manager_UI.FoodSlotController.UpdateItemsState(DataManager.Instance.PlayerAccount.Inventory.SelectedFood.ToArray());
                        }
                    }
                }
                else 
                {
                    int amount = DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(selectedFoodType);

                    //Найти UI слот и обновить количество предметов
                    GameManager.Instance.Manager_UI.FoodSlotController.UpdateItemAmount(selectedFoodType, amount);

                    //Найти UI слот и обновить прогресс
                    GameManager.Instance.Manager_UI.FoodSlotController.UpdateItemProgress(selectedFoodType, GetFoodProgress(amount)); 
                }

                return 10;
            }

            return 0;
        }


        protected override List<DataTableItems.ItemTypes> GetTargetItemList()
        {
            return DataManager.Instance.PlayerAccount.Inventory.SelectedFood;
        }

        protected override DataTableItems.ItemTypes GetDefaultItem()
        {
            return DataTableItems.ItemTypes.Max;
        }


        float GetFoodProgress(int amount)
        {
            return (float)amount / DataManager.Instance.PlayerAccount.Inventory.FoodState.MaxFoodInSlot;
        }
    }
}
