using clicker.datatables;
using clicker.general;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace clicker.battle
{
    public class WeaponManager : SelectedItemsManager
    {
        public override void Init()
        {
            //UI
            GameManager.Instance.Manager_UI.CreateWeaponSlots(DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.ToArray(),
                                                              GameManager.Instance.Manager_UI.UIParent_MiddleLeft,
                                                              true,
                                                              true);

            base.Init();
        }

        /// <summary>
        /// Выбрать оружие
        /// </summary>
        /// <param name="index">Индекс оружия из списка выбранного</param>
        public override void SelectItem(int index)
        {
            if (m_SelectedIndex == index)
                return;

            base.SelectItem(index);

            //Обновить UI - выделение слота
            GameManager.Instance.Manager_UI.WeaponSlotController.SelectItem(m_SelectedIndex);

            Debug.Log("WeaponManager: SELECT WEAPON. Slot: " + m_SelectedIndex + "." + 
                                                   " Type: " + GetItemTypeByIndex(m_SelectedIndex) + "." + 
                                                   " Amount: " + DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(GetItemTypeByIndex(m_SelectedIndex)));
        }

        /// <summary>
        /// Использовать выбранное оружие
        /// </summary>
        /// <returns>Урон от выбранного оружия</returns>
        public override int UseItem()
        {
            //Получить тип предмета
            DataTableItems.ItemTypes selectedWeaponType = GetItemTypeByIndex(m_SelectedIndex);

            //Если оружие было использовано
            if (DataManager.Instance.PlayerAccount.Inventory.WeaponState.UseWeapon(selectedWeaponType))
                return DataTableWeapons.GetWeaponDataByType(selectedWeaponType).Damage;

            return 0;
        }

        public override void UnscribeFromGlobalEvents()
        {
            base.UnscribeFromGlobalEvents();

            DataManager.Instance.PlayerAccount.Inventory.WeaponState.OnUseWeapon -= UseWeaponHandler;
            DataManager.Instance.PlayerAccount.Inventory.WeaponState.OnWeaponBroken -= BrokeWeaponHandler;
            DataManager.Instance.PlayerAccount.Inventory.WeaponState.OnRemoveWeapon -= RemoveWeaponHandler;
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(50);
            strBuilder.AppendFormat("Selected weapon: {0}", DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.Count);

            if (DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.Count > 0)
            {
                strBuilder.Append("\n");
                for (int i = 0; i < DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.Count; i++)
                    strBuilder.AppendFormat(" - {0}\n", DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon[i]);
            }

            return strBuilder.ToString();
        }


        protected override void SubcribeForGlobalEvents()
        {
            base.SubcribeForGlobalEvents();

            //Подписаться на события изменения состояния оружия
            DataManager.Instance.PlayerAccount.Inventory.WeaponState.OnUseWeapon += UseWeaponHandler;
            DataManager.Instance.PlayerAccount.Inventory.WeaponState.OnWeaponBroken += BrokeWeaponHandler;
            DataManager.Instance.PlayerAccount.Inventory.WeaponState.OnRemoveWeapon += RemoveWeaponHandler;
        }

        protected override void SubscribeForLocalEvents()
        {
            base.SubscribeForLocalEvents();

            //Подписать UI на события
            OnAddItem += GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemsState;
            OnRemoveItem += GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemsState;
            OnAddSlot += GameManager.Instance.Manager_UI.WeaponSlotController.AddSlot;
        }

        protected override List<DataTableItems.ItemTypes> GetTargetItemList()
        {
            return DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon;
        }


        /// <summary>
        /// Обработчик события использования оружия (Для обновления UI)
        /// </summary>
        /// <param name="weaponType">Тип использованного оружия</param>
        /// <param name="durabilityProgress">Текущий прогресс до поломки</param>
        void UseWeaponHandler(DataTableItems.ItemTypes weaponType, float durabilityProgress)
        {
            Debug.Log("WeaponManager: USE WEAPON: " + weaponType + ". Durability: " + durabilityProgress);

            //Найти слот и обновить прогресс
            GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemProgress(weaponType, durabilityProgress);
        }

        /// <summary>
        /// Обработчик поломки используемого оружия (Для обновления UI)
        /// </summary>
        /// <param name="weaponType">Тип использованного оружия</param>
        void BrokeWeaponHandler(DataTableItems.ItemTypes weaponType)
        {
            Debug.LogWarning("WeaponManager: BROKE WEAPON: " + weaponType + 
                           ". Total Amount: " + DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(weaponType) + 
                           ". Amount in Bag: " + DataManager.Instance.PlayerAccount.Inventory.BagsState.GetItemAmountInBag(weaponType));

            //Переключить на оружие по умолчанию
            if (!DataManager.Instance.PlayerAccount.Inventory.BagsState.HasItemInBag(weaponType))
            {
                //Найти слот с оружием, которое использовалось
                for (int i = 0; i < DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.Count; i++)
                {
                    //Заменить оружие в слоте на оружие по-умолчанию
                    if (DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon[i].Equals(weaponType))
                    {
                        DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon[i] = account.Account.AccountInventory.DEFAULT_ITEM;

                        //Найти UI слот и обновить оружие
                        GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemsState(DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.ToArray());
                    }
                }
            }
            else
            {
                //Найти UI слот и обновить количество предметов
                int amount = DataManager.Instance.PlayerAccount.Inventory.BagsState.GetItemAmountInBag(weaponType);
                GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemAmount(weaponType, amount);

                //Найти UI слот и обновить прочность
                float progress = DataManager.Instance.PlayerAccount.Inventory.WeaponState.GetDurabilityProgress(weaponType);
                GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemProgress(weaponType, progress);
            }
        }

        /// <summary>
        /// Удалить оружие из выбранного
        /// </summary>
        /// <param name="weaponType"></param>
        void RemoveWeaponHandler(DataTableItems.ItemTypes weaponType)
        {
            for (int i = 0; i < DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.Count; i++)
            {
                if (DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon[i].Equals(weaponType))
                    DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon[i] = account.Account.AccountInventory.DEFAULT_ITEM;
            }

            OnRemoveItem?.Invoke(DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.ToArray());
        }

        void Update()
        {
            //Test
            if (Input.GetKeyDown(KeyCode.U))
                UseItem();

            if (Input.GetKeyDown(KeyCode.S))
                AddSlot();
        }
    }
}
