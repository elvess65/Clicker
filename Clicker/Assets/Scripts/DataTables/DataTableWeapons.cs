using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static clicker.datatables.DataTableItems;

namespace clicker.datatables
{
    public class DataTableWeapons 
    {
        private static Dictionary<ItemTypes, Weapon> m_Weapons;

        public static void SetData(LocalWeaponsDataEditor.WeaponsData[] data)
        {
            if (data == null)
                return;

            m_Weapons = new Dictionary<ItemTypes, Weapon>();

            for (int i = 0; i < data.Length; i++)
            {
                //Создать предмет
                Weapon item = new Weapon(data[i].Type, data[i].Damage, data[i].Durability);

                m_Weapons.Add(data[i].Type, item);
            }
        }

        /// <summary>
        /// Получить данные об оружии по типу
        /// </summary>
        /// <param name="type">Тип предмета</param>
        /// <returns>Данные о предмете</returns>
        public static Weapon GetWeaponDataByType(ItemTypes type)
        {
            if (m_Weapons.ContainsKey(type))
                return m_Weapons[type];

            return null;
        }

        #region Data Structures
        /// <summary>
        /// Представляет описание оружия (тип оружия, урон и прочность)
        /// </summary>
        public class Weapon
        {
            private ItemTypes m_Type;                           //Тип предмета
            private int m_Damage;                               //Урон 
            private int m_Durability;                           //Прочность

            public ItemTypes Type => m_Type;
            public int Damage => m_Damage;
            public int Durability => m_Durability;

            public Weapon(ItemTypes type, int damage, int durability)
            {
                m_Type = type;
                m_Damage = damage;
                m_Durability = durability;
            }

            public override string ToString()
            {
                StringBuilder strBuilder = new StringBuilder(50);

                //Вывести общие данные 
                strBuilder.AppendFormat("Weapon: {0}. Damage: {1}. Durability: {2}", m_Type, m_Damage, m_Durability);

                return strBuilder.ToString();
            }
        }
        #endregion
    }
}