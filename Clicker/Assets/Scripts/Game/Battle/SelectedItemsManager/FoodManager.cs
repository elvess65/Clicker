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



            base.Init();
        }

        /// <summary>
        /// Выбрать еду
        /// </summary>
        /// <param name="index">Индекс еды из списка выбранного</param>
        public override void SelectItem(int index)
        {
            HandleDoubleClick(index);

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
                //Удалить использованный предмет из сумки
                DataManager.Instance.PlayerAccount.Inventory.BagsState.RemoveItemFromBag(selectedFoodType, false);

                Debug.Log("Has Amount: " + DataManager.Instance.PlayerAccount.Inventory.HasItem(selectedFoodType));
                Debug.Log("Has Amount in bag: " + DataManager.Instance.PlayerAccount.Inventory.BagsState.HasItemInBag(selectedFoodType));

                //Если после использования предмета в сумке не осталось предметов
                if (!DataManager.Instance.PlayerAccount.Inventory.BagsState.HasItemInBag(selectedFoodType))
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
                    int amount = DataManager.Instance.PlayerAccount.Inventory.BagsState.GetItemAmountInBag(selectedFoodType);

                    //Найти UI слот и обновить количество предметов
                    GameManager.Instance.Manager_UI.FoodSlotController.UpdateItemAmount(selectedFoodType, amount);

                    //Найти UI слот и обновить прогресс
                    GameManager.Instance.Manager_UI.FoodSlotController.UpdateItemProgress(selectedFoodType, 
                                                                                          DataManager.Instance.PlayerAccount.Inventory.BagsState.GetFillBagProgress(selectedFoodType, 
                                                                                                                                                                    DataTableItems.ItemFilterTypes.Food));
                }

                return 1;
            }

            return 0;
        }


        protected override void SubscribeForLocalEvents()
        {
            base.SubscribeForLocalEvents();

            //Подписать UI на событие
            OnAddItem += GameManager.Instance.Manager_UI.FoodSlotController.UpdateItemsState;
            OnRemoveItem += GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemsState;
            OnAddSlot += GameManager.Instance.Manager_UI.FoodSlotController.AddSlot;
        }

        protected override List<DataTableItems.ItemTypes> GetTargetItemList()
        {
            return DataManager.Instance.PlayerAccount.Inventory.SelectedFood;
        }

        protected override DataTableItems.ItemTypes GetDefaultItem()
        {
            return DataTableItems.ItemTypes.Max;
        }


        void HandleDoubleClick(int index)
        {
            //Если только выделили ячеыйку - очистить время предыдущего выделения
            if (m_SelectedIndex != index)
                m_LastClickTime = 0;

            //Двойное нажатие
            float timeBtwClick = Time.time - m_LastClickTime;
            m_LastClickTime = Time.time;
            if (timeBtwClick <= m_DOUBLE_CLICK_TIME)
            {
                if (GameManager.Instance.Manager_Battle.Player.HPController.CurrentProgress < 1)
                    GameManager.Instance.Manager_Battle.Player.HPController.AddHP(UseItem());

                m_LastClickTime = 0;
            }
        }
    }
}
