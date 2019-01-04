using clicker.datatables;
using clicker.general;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace clicker.battle
{
    public class WeaponManager : MonoBehaviour
    {
        private int m_SelectedIndex = -1;

        private List<DataTableItems.ItemTypes> m_SelectedWeapons;

        private void Start()
        {
            //Test
            //Используемое оружие
            m_SelectedWeapons = new List<DataTableItems.ItemTypes>();
            m_SelectedWeapons.Add(DataTableItems.ItemTypes.Hand);
            m_SelectedWeapons.Add(DataTableItems.ItemTypes.Stone);

            GameManager.Instance.PlayerAccount.Inventory.WeaponState.OnUseWeapon += UseWeaponHandler;
            GameManager.Instance.PlayerAccount.Inventory.WeaponState.OnWeaponBroken += BrokeWeaponHandler;

            SelectWeapon(1);
        }

        private void Update()
        {
            //Test
            if (Input.GetKeyDown(KeyCode.Space))
                Debug.Log("Take damage: " + UseWeapon());

            if (Input.GetKeyDown(KeyCode.Alpha1))
                SelectWeapon(0);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                SelectWeapon(1);
        }

        /// <summary>
        /// Использовать выбранное оружие
        /// </summary>
        /// <returns>Урон от выбранного оружия</returns>
        public int UseWeapon()
        {
            //Получить тип оружия
            DataTableItems.ItemTypes selectedWeaponType = GetWeaponTypeByIndex(m_SelectedIndex);

            //Если оружие было использовано
            if (GameManager.Instance.PlayerAccount.Inventory.WeaponState.UseWeapon(selectedWeaponType))
                return DataTableWeapons.GetWeaponDataByType(selectedWeaponType).Damage;

            return 0;
        }


        /// <summary>
        /// Обработчик события использования оружия (Для обновления UI)
        /// </summary>
        /// <param name="weaponType">Тип использованного оружия</param>
        /// <param name="durabilityProgress">Текущий прогресс до поломки</param>
        void UseWeaponHandler(DataTableItems.ItemTypes weaponType, float durabilityProgress)
        {
            Debug.Log("WeaponManager: USE WEAPON: " + weaponType + ". Durability: " + durabilityProgress);

            //Обновить UI
            //Найти слот и обновить прогресс
        }

        /// <summary>
        /// Обработчик поломки используемого оружия (Для обновления UI)
        /// </summary>
        /// <param name="weaponType">Тип использованного оружия</param>
        void BrokeWeaponHandler(DataTableItems.ItemTypes weaponType)
        {
            Debug.Log("WeaponManager: BROKE WEAPON: " + weaponType + ". Amount: " + GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(weaponType));

            //Переключить на оружие по умолчанию
            //Обновить UI
            if (GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(weaponType) == 0)
            {
                //Найти слот с оружием, которое использовалось
                for (int i = 0; i < m_SelectedWeapons.Count; i++)
                {
                    //Заменить оружие в слоте на оружие по-умолчанию
                    if (m_SelectedWeapons[i].Equals(weaponType))
                    {
                        m_SelectedWeapons[i] = DataTableItems.ItemTypes.Hand;

                        //Обновить UI
                        //Найти слот и заменить иконку
                    }
                }
            }
            else
            {
                //Обновить UI
                //Найти слот и обновить количество
            }
        }


        void SelectWeapon(int index)
        {
            if (m_SelectedIndex == index)
                return;

            m_SelectedIndex = index;

            Debug.Log("WeaponManager: SELECT WEAPON. Slot: " + m_SelectedIndex + " Type: " + GetWeaponTypeByIndex(m_SelectedIndex) + " Amount: " + GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(GetWeaponTypeByIndex(m_SelectedIndex)));
            //Обновить UI - выделение слота
        }

        DataTableItems.ItemTypes GetWeaponTypeByIndex(int index)
        {
            try
            {
                return m_SelectedWeapons[index];
            }
            catch
            { }

            return DataTableItems.ItemTypes.Hand;
        }
    }
}
