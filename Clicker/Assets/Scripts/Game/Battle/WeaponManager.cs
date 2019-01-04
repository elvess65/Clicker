using clicker.datatables;
using clicker.general;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace clicker.battle
{
    public class WeaponManager : MonoBehaviour
    {
        public DataTableItems.ItemTypes SelectedWeaponType { get; private set; }

        private void Start()
        {
            GameManager.Instance.PlayerAccount.Inventory.WeaponState.OnUseWeapon += UseWeaponHandler;
            GameManager.Instance.PlayerAccount.Inventory.WeaponState.OnWeaponBroken += BrokeWeaponHandler;

            SelectedWeaponType = DataTableItems.ItemTypes.Stone;
        }

        void UseWeaponHandler(DataTableItems.ItemTypes weaponType, float durabilityProgress)
        {
            Debug.Log("WeaponManager: USE WEAPON: " + weaponType + ". Durability: " + durabilityProgress);

            //Обновить UI
        }

        void BrokeWeaponHandler(DataTableItems.ItemTypes weaponType)
        {
            Debug.Log("WeaponManager: BROKE WEAPON: " + weaponType + ". Amount: " + GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(weaponType));

            //Переключить на оружие по умолчанию
            //Обновить UI
            if (GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(weaponType) == 0)
                SelectedWeaponType = DataTableItems.ItemTypes.Hand;
        }

        private void Update()
        {
            //Test
            if (Input.GetKeyDown(KeyCode.Space))
                Debug.Log("Take damage: " + UseWeapon());
        }

        /// <summary>
        /// Использовать выбранное оружие
        /// </summary>
        /// <returns>Урон от выбранного оружия</returns>
        public int UseWeapon()
        {
            DataTableWeapons.Weapon weaponData = DataTableWeapons.GetWeaponDataByType(SelectedWeaponType);

            if (GameManager.Instance.PlayerAccount.Inventory.WeaponState.UseWeapon(SelectedWeaponType))
                return weaponData.Damage;

            return 0;
        }

        /// <summary>
        /// Текущее 
        /// </summary>
        private class WeaponData
        {
            private DataTableItems.ItemTypes m_Type;
            private int m_Damage;
            private int m_Durability;

            public WeaponData(DataTableItems.ItemTypes type, int damage, int durability)
            {
                m_Type = type;
                m_Damage = damage;
                m_Durability = durability;
            }
        }
    }
}
