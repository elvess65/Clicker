using clicker.datatables;
using clicker.general;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace clicker.battle
{
    public class WeaponManager : MonoBehaviour
    {
        private int m_SelectedIndex = -1;

        public List<DataTableItems.ItemTypes> SelectedWeapons { get; private set; }

        public void Init(DataTableItems.ItemTypes[] selectedWeapons)
        {
            //Заполнить список выбранного оружия
            SelectedWeapons = new List<DataTableItems.ItemTypes>();
            for (int i = 0; i < selectedWeapons.Length; i++)
                SelectedWeapons.Add(selectedWeapons[i]);

            //Подписаться на события изменения состояния оружия
            GameManager.Instance.PlayerAccount.Inventory.WeaponState.OnUseWeapon += UseWeaponHandler;
            GameManager.Instance.PlayerAccount.Inventory.WeaponState.OnWeaponBroken += BrokeWeaponHandler;

            //UI
            GameManager.Instance.Manager_UI.CreateWeaponSlots(selectedWeapons, GameManager.Instance.Manager_UI.UIParent_MiddleLeft, true, true);
            StartCoroutine(WaitFrameToSelectWeapon(1));
        }

        /// <summary>
        /// Выбрать оружие
        /// </summary>
        /// <param name="index">Индекс оружия из списка выбранного</param>
        public void SelectWeapon(int index)
        {
            if (m_SelectedIndex == index)
                return;

            //Изменить индекс выделенного оружия
            m_SelectedIndex = index;

            //Обновить UI - выделение слота
            GameManager.Instance.Manager_UI.WeaponSlotController.SelectItem(m_SelectedIndex);

            Debug.Log("WeaponManager: SELECT WEAPON. Slot: " + m_SelectedIndex + ". Type: " + GetWeaponTypeByIndex(m_SelectedIndex) + " Amount: " + GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(GetWeaponTypeByIndex(m_SelectedIndex)));
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
        /// Добавить оружие в слот
        /// </summary>
        /// <param name="index">Индекс слота</param>
        /// <param name="weaponType">Тип оружия</param>
        public void AddWeapon(int index, DataTableItems.ItemTypes weaponType)
        {
            try
            {
                SelectedWeapons[index] = weaponType;

                Debug.Log(ToString());

                //Обновить глобальное UI
                GameManager.Instance.Manager_UI.WeaponSlotController.ReplaceWeaponInSlot(index, weaponType);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Exeption: " + e.Message);
            }
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(50);
            strBuilder.AppendFormat("Selected weapon: {0}", SelectedWeapons.Count);

            if (SelectedWeapons.Count > 0)
            {
                strBuilder.Append("\n");
                for (int i = 0; i < SelectedWeapons.Count; i++)
                    strBuilder.AppendFormat(" - {0}\n", SelectedWeapons[i]);
            }

            return strBuilder.ToString();
        }


        /// <summary>
        /// Обработчик события использования оружия (Для обновления UI)
        /// </summary>
        /// <param name="weaponType">Тип использованного оружия</param>
        /// <param name="durabilityProgress">Текущий прогресс до поломки</param>
        void UseWeaponHandler(DataTableItems.ItemTypes weaponType, float durabilityProgress)
        {
            Debug.LogWarning("WeaponManager: USE WEAPON: " + weaponType + ". Durability: " + durabilityProgress);

            //Обновить UI
            //Найти слот и обновить прогресс
            GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemDurability(weaponType, durabilityProgress);
        }

        /// <summary>
        /// Обработчик поломки используемого оружия (Для обновления UI)
        /// </summary>
        /// <param name="weaponType">Тип использованного оружия</param>
        void BrokeWeaponHandler(DataTableItems.ItemTypes weaponType)
        {
            Debug.LogError("WeaponManager: BROKE WEAPON: " + weaponType + 
                           ". Amount: " + GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(weaponType));

            //Переключить на оружие по умолчанию
            //Обновить UI
            if (GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(weaponType) == 0)
            {
                //Найти слот с оружием, которое использовалось
                for (int i = 0; i < SelectedWeapons.Count; i++)
                {
                    //Заменить оружие в слоте на оружие по-умолчанию
                    if (SelectedWeapons[i].Equals(weaponType))
                    {
                        SelectedWeapons[i] = DataTableItems.ItemTypes.Hand;

                        //Найти слот и заменить иконку
                        GameManager.Instance.Manager_UI.WeaponSlotController.ReplaceWeapon(weaponType, SelectedWeapons[i]);
                    }
                }
            }
            else
            {
                //Найти слот и обновить количество предметов
                GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemAmount(weaponType, GameManager.Instance.PlayerAccount.Inventory.GetItemAmount(weaponType));
                
                //Найти слот и обновить прочность
                GameManager.Instance.Manager_UI.WeaponSlotController.UpdateItemDurability(weaponType, GameManager.Instance.PlayerAccount.Inventory.WeaponState.GetDurabilityProgress(weaponType));
            }
        }

        DataTableItems.ItemTypes GetWeaponTypeByIndex(int index)
        {
            try
            {
                return SelectedWeapons[index];
            }
            catch
            { }

            return DataTableItems.ItemTypes.Hand;
        }

        IEnumerator WaitFrameToSelectWeapon(int index)
        {
            yield return null;
            SelectWeapon(index);
        }

        void Update()
        {
            //Test
            if (Input.GetKeyDown(KeyCode.U))
                Debug.Log("Take damage: " + UseWeapon());
        }
    }
}
